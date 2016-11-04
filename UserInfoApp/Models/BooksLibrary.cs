using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types


public class BookEntity : TableEntity
{
    public BookEntity(string partitionName, string bookName)
    {

        this.PartitionKey = partitionName;
        this.RowKey = bookName;
    }

    public BookEntity() { }

    public string Availability { get; set; }
    public string UserName { get; set; }

}