#!/usr/bin/env bash
function red {
  echo -e "\033[1;31m$1\033[0m	$2"
}

function green {
  echo -e "\033[1;32m$1\033[0m	$2"
}

function yellow {
  echo -e "\033[1;33m$1\033[0m	$2"
}

ROOT=`git rev-parse --show-toplevel`
cd $ROOT

OLD_TAG=`git describe --tags --abbrev=0 2>/dev/null`
CURRENT_TAG=`git describe --tags 2>/dev/null`
OLD_VERSION=`cat VERSION`
NEW_VERSION=`expr $OLD_VERSION + 1`

set -e # Exit on nonzero return status

if [ -z "$OLD_TAG" ]; then
  echo "No existing tags, enter a new version (e.g. 1.0.0)"
else
  echo -n "Last explicit tag: "
  yellow $OLD_TAG
  echo -n "Current tag: "
  yellow $CURRENT_TAG
fi

if [ -z "$OLD_VERSION" ]; then
  echo "No old version, starting 1"
  echo 1 > VERSION
else
  echo -n "Last version: "
  yellow $OLD_VERSION
  echo -n "New version: "
  green $NEW_VERSION
fi

echo -n "Enter the new tag: "
read NEW_TAG

echo -n "New tag: "
green $NEW_TAG

echo -n "Does this look OK? [Press 'y'] "
read -n 1 CONFIRM
echo

if [ "$CONFIRM" != "y" ]; then
  red "Stopping"
  exit 1
fi
echo

echo -n "Checking for dirty git tree... "
if test -n "$(git status --porcelain)"; then
  red "Uncommitted changes!"
  exit 1
fi
green "OK"

echo -n "Tagging release... "
git tag --annotate $NEW_TAG -m "Version $NEW_TAG"
green "OK"

echo -n "Updating release... "
sed -i '' "s/^  bundleVersion: .*/  bundleVersion: $NEW_TAG/g" ./ProjectSettings/ProjectSettings.asset
sed -i '' "s/^  AndroidBundleVersionCode: .*/  AndroidBundleVersionCode: $NEW_VERSION/g" ./ProjectSettings/ProjectSettings.asset
echo $NEW_VERSION > VERSION
green "OK"

echo -n "Creating release dir if it doesn't exist... "
mkdir -p Releases
green "OK"

echo -n "Outputting release notes... "
git log --abbrev-commit --pretty=oneline --reverse \
  $OLD_TAG..HEAD >> Releases/sOS-${NEW_TAG}-notes.txt
green "OK"

echo -n "Comitting changes... "
git add VERSION Releases ./ProjectSettings/ProjectSettings.asset
git commit -m "New release - $NEW_TAG"
green "OK"

echo -n "Updating tag... "
git tag --annotate --force $NEW_TAG -m "Version $NEW_TAG"
green "OK"

echo -n "Pushing to origin... "
git push origin
git push origin --tags
green "OK"
