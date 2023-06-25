#!/usr/bin/env bash

base_dest_path=/usr/share/games/Prince_of_Unity
desktop_launcher=ninja.dlab.Prince_of_Unity.desktop

# Link executable from /usr/games to /usr/share/games/Prince_of_Unity.
ln -s $base_dest_path/Prince_of_Unity /usr/games/Prince_of_Unity

# Copy desktop entry
cp $base_dest_path/$desktop_launcher /usr/share/applications/.
