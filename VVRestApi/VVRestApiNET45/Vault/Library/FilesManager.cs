﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Library
{
    public class FilesManager : VVRestApi.Common.BaseApi
    {

        internal FilesManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }


        public JObject UploadFile(Guid documentId, string filename, string revision, string changeReason, DocumentCheckInState checkInState, List<KeyValuePair<string, string>> indexFields, byte[] file)
        {
            if (documentId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "documentId");
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("filename is required but was empty", "filename");
            }
            var jobject = new JObject();
            foreach (var indexField in indexFields)
            {
                jobject.Add(new JProperty(indexField.Key, indexField.Value));
            }
            var jobjectString = JsonConvert.SerializeObject(jobject);

            var postData = new List<KeyValuePair<string, string>>
            {       
                new KeyValuePair<string, string>("documentId", documentId.ToString()),
                new KeyValuePair<string, string>("filename", filename),
                new KeyValuePair<string, string>("revision", revision),
                new KeyValuePair<string, string>("changeReason", changeReason),
                new KeyValuePair<string, string>("checkInDocumentState", checkInState.ToString()),
                new KeyValuePair<string, string>("indexFields", jobjectString)
            };

            return HttpHelper.PostMultiPart(GlobalConfiguration.Routes.Files, "", GetUrlParts(), this.ApiTokens, postData, filename, file);
        }

        public JObject UploadFile(Guid documentId, string fileName, string revision, string changeReason, DocumentCheckInState checkInState, List<KeyValuePair<string, string>> indexFields, Stream fileStream, JObject optionalParameters = null)
        {
            if (documentId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "documentId");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("DocumentId is required but was empty", "fileName");
            }

            var jobject = new JObject();
            foreach (var indexField in indexFields)
            {
                jobject.Add(new JProperty(indexField.Key, indexField.Value));
            }
            var jobjectString = JsonConvert.SerializeObject(jobject);

            var postData = new List<KeyValuePair<string, string>>
            {       
                new KeyValuePair<string, string>("documentId", documentId.ToString()),
                new KeyValuePair<string, string>("fileName", fileName),
                new KeyValuePair<string, string>("revision", revision),
                new KeyValuePair<string, string>("changeReason", changeReason),
                new KeyValuePair<string, string>("checkInDocumentState", checkInState.ToString()),
                new KeyValuePair<string, string>("indexFields", jobjectString)
            };

            if (optionalParameters != null)
            {
                postData.Add(new KeyValuePair<string, string>("parameters", optionalParameters.ToString()));
            }

            return HttpHelper.PostMultiPart(GlobalConfiguration.Routes.Files, "", GetUrlParts(), this.ApiTokens, postData, fileName, fileStream);
        }

        /// <summary>
        /// Gets a folder by its path, returns null if none exists
        /// </summary>
        /// <param name="documentRevisionId"></param>
        /// <param name="options"> </param>
        /// <returns></returns>
        public Stream GetStream(Guid documentRevisionId, RequestOptions options = null)
        {
            return HttpHelper.GetStream(VVRestApi.GlobalConfiguration.Routes.FilesId, "", options, GetUrlParts(), this.ApiTokens, this.ClientSecrets, documentRevisionId);
        }

        public JObject UploadZeroByteFile(Guid documentId, string fileName, long fileSize, byte[] file)
        {
            if (documentId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "documentId");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("DocumentId is required but was empty", "fileName");
            }

            dynamic newFile = new ExpandoObject();
            newFile.documentId = documentId.ToString();
            newFile.fileName = fileName;
            newFile.allowNoFile = true;
            newFile.checkInDocumentState = DocumentCheckInState.Replace.ToString();
            newFile.fileLength = fileSize.ToString();
            newFile.revision = "1";
            newFile.changeReason = "Test zero byte file upload - Replace Revision";

            return HttpHelper.Post(GlobalConfiguration.Routes.FilesNoFileAllowed, "", GetUrlParts(), this.ApiTokens, newFile, fileName, file);
        }




    }
}
