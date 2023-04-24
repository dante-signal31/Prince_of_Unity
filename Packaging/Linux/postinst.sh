#!/usr/bin/env bash

# Link executable from /usr/games to /usr/share/games/Prince_of_Unity.
ln -s /usr/share/games/Prince_of_Unity/Prince_of_Unity /usr/games/Prince_of_Unity

# Copy desktop entry
cp /usr/share/games/Prince_of_Unity/ninja.dlab.Prince_of_Unity.desktop /usr/share/applications/ninja.dlab.Prince_of_Unity.desktop
