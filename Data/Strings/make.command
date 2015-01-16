echo Build Strings

BASEDIR=$(dirname $0)
XLS="./Strings.xls"
RESOURCES="../../Assets/Resources/strings/"
SOURCES="../../Assets/MotinGames/StringManager/Generated/"

cd $BASEDIR
python buildStringPack.py $XLS $RESOURCES $SOURCES
