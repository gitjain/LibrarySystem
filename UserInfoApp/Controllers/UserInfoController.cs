using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using System.Web.Http;

namespace UserInfoApp.Controllers
{
    public class UserInfoController : ApiController
    {
        public IHttpActionResult GetUserInfo()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                WebConfigurationManager.AppSettings.Get("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "userInfo" table.
            CloudTable table = tableClient.GetTableReference("userInfo");

            // Construct the query operation for all user entities.
            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "userInfo"));

            List<String> list = new List<string>();

            foreach (UserEntity entity in table.ExecuteQuery(query))
            {
                list.Add(entity.firstName + " " + entity.lastName);
            }

            return Ok(list);
        }

        public IHttpActionResult PutUserInfo(string userName, string firstName, string lastName)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               WebConfigurationManager.AppSettings.Get("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "userInfo" table.
            CloudTable table = tableClient.GetTableReference("userInfo");

            // Create a new user entity.
            UserEntity user = new UserEntity("userInfo", userName);
            user.firstName = firstName;
            user.lastName = lastName;
            

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(user);

            // Execute the insert operation.
            TableResult response = null;
            try
            {
               response = table.Execute(insertOperation);
            }
            catch (Exception)
            {
      
            }

            if (response == null)
            {
                return Ok("User already exists");
            }
            return Ok("User Info added successfully");
        }

        public IHttpActionResult GetLibraryUser(string name)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            WebConfigurationManager.AppSettings.Get("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "userInfo" table.
            CloudTable table = tableClient.GetTableReference("userInfo");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>("userInfo", name);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
                return Ok(((UserEntity)retrievedResult.Result).RowKey);
            else
                return Ok("The user does not exist.");
        }
    }
}
