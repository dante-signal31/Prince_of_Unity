#!/usr/bin/env bash

desktop_launcher=ninja.dlab.Prince_of_Unity.desktop

# Remove link in /usr/games
rm /usr/games/Prince_of_Unity

# Remove desktop entry.
rm /usr/share/applications/$desktop_launcher