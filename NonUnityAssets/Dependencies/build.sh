#!/usr/bin/env bash

cd `git rev-parse --show-toplevel`/NonUnityAssets/Dependencies

#dot -Kneato -Tpng -O chart.dot
dot -Tpng -O chart.dot
open chart.dot.png
