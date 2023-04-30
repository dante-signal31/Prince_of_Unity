######
# Script main configuration.
######
# Debian does not like capital letters nor underscores in package names, so lets don't use them.
app=prince-of-unity
# Provide version as an script argument.
version=$1
architecture=amd64
source_directory=./
package_install_root=/usr/share/games/Prince_of_Unity
output_folder=Installers/Linux
after_install=Packaging/Linux/postinst.sh
after_remove=Packaging/Linux/postuninst.sh
maintainer=dante.signal31@gmail.com
url=https://github.com/dante-signal31/Prince_of_Unity
description="Developed with Unity, this game is a hobbyist black box demo remake of Jordan Mechner's classic Prince of Persia (1989)."
license=BSD-3  
category=games

######
# Control printing.
######
fpm_parameters="-s dir -t deb -C Build -n $app \
               -p $output_folder -v \"$version\" \
               --prefix $package_install_root \
               --description \"$description\" \
               --after-install $after_install --after-remove $after_remove \
               --maintainer $maintainer -a $architecture --url $url \
               --license $license --category $category \
               $source_directory"
               
echo "Current dir: $(pwd)"
echo "Current version: $version"
echo "Calling fpm with this parameters: $fpm_parameters"

######
# Copy needed resources to built game folder.
######
cp Packaging/Linux/ninja.dlab.Prince_of_Unity.desktop Build/.
cp Assets/Icons/Running_icon.png Build/Prince_of_Unity_icon.png

######
# Fix folder permissions.
######
chmod 755 Build/'Prince of Unity'
chmod 755 Build/'Prince of Unity_Data'
chmod 755 Build/'Prince of Unity_Data'/il2cpp_data
chmod 755 Build/'Prince of Unity_Data'/il2cpp_data/Metadata
chmod 755 Build/'Prince of Unity_Data'/il2cpp_data/Resources
chmod 755 Build/'Prince of Unity_Data'/Resources

######
# Run FPM command.
######
fpm -s dir -t deb -C Build -n $app \
-p $output_folder -v "$version" \
--prefix $package_install_root \
--description "$description" \
--after-install $after_install --after-remove $after_remove \
--maintainer $maintainer -a $architecture --url $url \
--license $license --category $category \
$source_directory

fpm -s dir -t rpm -C Build -n $app \
-p $output_folder -v "$version" \
--prefix $package_install_root \
--description "$description" \
--after-install $after_install --after-remove $after_remove \
--maintainer $maintainer -a $architecture --url $url \
--license $license --category $category \
$source_directory