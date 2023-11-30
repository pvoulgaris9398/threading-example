# Threading Example
## Overview
This application will manage `n` threads, which will write out messages to an output (log) file.
Each thread will have `m` iterations of writing to this output (log) file.
The implementation makes use of the C# `lock` method to ensure only one thread can write to the output file at a time.
