#!/bin/bash 

echo "Enter migration name:"
read name
dotnet-ef migrations add $name -- --host