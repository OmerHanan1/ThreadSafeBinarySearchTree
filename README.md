In order to be able to use the tree in multi-threading mode (more than one user parallel), \
we had to treat the BST itself and is nodes as a resource. \
Each operation on the BST will cause lock and release operations. \
</br>
For this job we used object called “ReaderWriterLockSlim” \
more information here – [Microsoft documentation](https://docs.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim?view=net-6.0.) \
</br>
This object, allow us to manage accesses to the resource in a way that Read key is allowed to multiply threads, but Write key is exclusive. \
Following the above method, combined with code structure that will shortly describe, we achieved managed synchronization access to the tree resources. \
</br>
<b>Example of code structure:</b> \
The tree has a single “ReaderWriterLockSlim” instance named “CaheLockSlim”. \
Each time some function is trying to access tree resource, \
we will try to enter the relevant key (remember multiple readers and exclusive write). \
After that we will use Try-Finally blocks, which will be used first (try) to make operations on resource and last (finally) release the lock. \
We use the Try-Finally approach because finally block runs at any case. This allows us to release lock no matter if some user (thread) fails. \
</br>
<b>Code examples:</b>
</br>
![image](https://user-images.githubusercontent.com/79142560/173925055-e50e43ed-d6b6-45f1-898c-36c17fd790df.png)
