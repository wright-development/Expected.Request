PROJECT_PATH="src/Expected.Request/Expected.Request.csproj"

VERSION_TAG=$(grep -o '<Version>.*</Version>' $PROJECT_PATH)
VERSION=$(echo $VERSION_TAG | grep -P -o '\d+\.\d+\.\d+')

MAJOR=$(echo $VERSION | cut -d '.' -f 1 )
MINOR=$(echo $VERSION | cut -d '.' -f 2 )
PATCH=$(echo $VERSION | cut -d '.' -f 3 )

UPDATED_PATCH=$(($PATCH + 1))

NEW_VERSION_TAG="<Version>$MAJOR.$MINOR.$UPDATED_PATCH</Version>"
echo $NEW_VERSION_TAG

sed -i -e "s#$VERSION_TAG#$NEW_VERSION_TAG#g" $PROJECT_PATH