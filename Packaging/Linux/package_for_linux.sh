app=Prince_of_Unity
version=$1
architecture=amd64
source_directory=Build
package_install_root=/usr/share/games/Prince_of_Unity
output_folder=Installers/Linux
after_install=Packaging/Linux/postinst.sh
after_remove=Packaging/Linux/postuninst.sh
maintainer=dante.signal31@gmail.com
url=https://github.com/dante-signal31/Prince_of_Unity
description="Developed with Unity, this game is a hobbyist black box demo remake of Jordan Mechner's classic Prince of Persia (1989)."
license=BSD-3  
category=games

fpm_parameters="-n $app \
               -p $output_folder -v \"$version\" \
               --description \"$description\" \
               --after-install $after_install --after-remove $after_remove \
               --maintainer $maintainer -a $architecture --url $url \
               --license $license --category $category \
               $source_directory"
               
echo "Current dir: $(pwd)"
echo "Current version: $version"
echo "Calling fpm with this parameters: $fpm_parameters"

#temp_dir=$(mktemp -d)
#echo "Temp folder created: $temp_dir"
#mkdir -p $temp_dir/$package_install_root
#cp -r $source_directory/* $temp_dir/$package_install_root/.

fpm -s dir -t deb -n $app \
-p $output_folder -v "$version" \
--prefix $package_install_root \
--description "$description" \
--after-install $after_install --after-remove $after_remove \
--maintainer $maintainer -a $architecture --url $url \
--license $license --category $category \
$source_directory/*