# MD5 Lister

This tool lists the files in a folder in the following format:

example of : "fileHashes.json"
```
[
  {
    "Path": "File.csv",
    "Hash": "88e284bf42d79db8aeb8be591d0179f8"
  }
]
```

The path initially entered is saved in a json file:

exemple of : "path.json"
```
{ "Path": "C:\Path\"}
```

So you don't have to retype the path every time you modify your folder.

I had a need and I didn't want to take what was already done elsewhere, that's all.

## Requirements
.NET 7
