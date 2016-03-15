#!/bin/bash

. ~/.dnx/dnvm/dnvm.sh
dnvm use 1.0.0-rc1-update1 -r mono
dnu restore --quiet
find . -name 'project.json' | while read project
do
  echo -------------------------------------------------------------------------
  echo "Mono Build $project"
  dnu build "$project" --quiet
done

dnvm use 1.0.0-rc1-update1 -r coreclr
dnu restore --quiet
diff <(find . -name 'project.json') exclude-coreclr.txt -w --changed-group-format='%<%>' --unchanged-group-format='' | while read project
do
  echo -------------------------------------------------------------------------
  echo "CoreCLR Build $project"
  dnu build "$project" --framework dnxcore50 --quiet
done
