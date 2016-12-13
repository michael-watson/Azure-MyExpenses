using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Files.Controllers;
using MyExpenses.Backend.DataObjects;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile.Server.Files;
using System.Collections.Generic;
using MyExpenses.Backend.Models;
using Microsoft.Azure.Mobile.Server.Tables;
using Microsoft.Azure.Mobile.Server;
using System.Web.Http.Controllers;

namespace MyExpenses.Backend.Controllers
{
    [MobileAppController]
    [RoutePrefix("tables/ExpenseModel/ReceiptImage")]
    public class ReceiptImageStorageController : StorageController<ExpenseModel>
    {
        // GET api/ReceiptImage
        [HttpPost]
        [Route("{id}/StorageToken")]
        public async Task<HttpResponseMessage> PostStorageTokenRequest([FromBody] string id, StorageTokenRequest value)
        {
            StorageToken token = await GetStorageTokenAsync(id, value);

            return Request.CreateResponse(token);
        }

        // Get the files associated with this record
        [HttpGet]
        [Route("{id}/MobileServiceFiles")]
        public async Task<HttpResponseMessage> GetFiles(string id)
        {
            IEnumerable<MobileServiceFile> files = await GetRecordFilesAsync(id);

            return Request.CreateResponse(files);
        }

        [HttpDelete]
        [Route("{id}/MobileServiceFiles/{name}")]
        public Task Delete(string id, string name)
        {
            return base.DeleteFileAsync(id, name);
        }
    }
}