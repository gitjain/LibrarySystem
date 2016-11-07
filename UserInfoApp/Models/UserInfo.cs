using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types


public class UserEntity : TableEntity
{
    public UserEntity(string partitionName, string userName)
    {

        this.PartitionKey = partitionName;
        this.RowKey = userName;
    }

    public UserEntity() { }

    public string firstName { get; set; }
    public string lastName { get; set; }
    
}