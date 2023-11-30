# Threading Example
## Overview
This application will manage `n` threads, which will write out messages to an output (log) file.
Each thread will have `m` iterations of writing to this output (log) file.
The implementation makes use of the C# `lock` method to ensure only one thread can write to the output file at a time.
The write-to-file functionality is encapsulated in such a way that the row numbers will remain sequential, despite multi-threaded access

## Prerequisites
* Docker desktop must be installed on your machine to use this image
* Docker can be downloaded from [here](https://docs.docker.com/desktop/install/windows-install/)

## Usage
```docker pull pvoulgaris9398/threading-example```
Followed by
```docker run -i -v c:\\junk:/log pvoulgaris9398/threading-example``` in a bash shell

or
```docker run -i -v c:\junk:/log pvoulgaris9398/threading-example``` from a Windows command prompt
