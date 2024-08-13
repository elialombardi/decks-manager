#!/bin/bash

# Define the directory
DIRECTORY="users"

# Function to replace 'deck' with 'users' while preserving the casing
replace_case_sensitive() {
    local original=$1
    local replacement=$2
    local string=$3

    # Use awk to replace while preserving the case
    echo "$string" | awk -v orig="$original" -v repl="$replacement" '
    {
        gsub(orig, repl, $0)
        gsub(tolower(orig), tolower(repl), $0)
        gsub(toupper(orig), toupper(repl), $0)
        gsub(toupper(substr(orig,1,1)) tolower(substr(orig,2)), toupper(substr(repl,1,1)) tolower(substr(repl,2)), $0)
        print
    }'
}

# Find all files in the directory and its subdirectories
find "$DIRECTORY" -type f | while read -r file; do
    # Get the directory and filename separately
    dir=$(dirname "$file")
    base=$(basename "$file")
    
    # Replace 'deck' with 'users' in the filename while preserving the casing
    new_base=$(replace_case_sensitive "deck" "users" "$base")
    
    # Rename the file
    mv "$file" "$dir/$new_base"
done

echo "Filename replacement complete."