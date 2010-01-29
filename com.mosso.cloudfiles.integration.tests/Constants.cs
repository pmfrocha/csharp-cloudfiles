namespace com.mosso.cloudfiles.integration.tests
{
    internal static class Constants
    {
        public const string XAuthUser = "X-Auth-User";
        public const string XAuthKey = "X-Auth-Key";
        public const string XStorageUrl = "X-Storage-Url";
        public const string XContainerObjectCount = "X-Container-Object-Count";
        public const string XContainerBytesUsed = "X-Container-Bytes-Used";
        public const string XMetaKeyHeader = "X-Object-Meta-";
        public const string XAccountContainerCount = "X-Account-Container-Count";
        public const string XAccountBytesUsed = "X-Account-Bytes-Used";
        public static string XCdnManagementUrl = "X-CDN-Management-URL";
        public static string XAuthToken = "X-Auth-Token";
        public static string XLogRetention = "X-Log-Retention";
        public const string ETAG = "ETag";
        public const string CONTAINER_NAME = "integration.tests.container.new";
         
        //Mosso accounts for authentication
        public const string MOSSO_AUTH_URL = "https://auth.api.rackspacecloud.com/v1.0";

        //Response constants
        public const int NoContentResponse = 204;
        public const int SuccessResponse = 200;
        public const int CreateSuccessResponse = 201;
        public const int ContainerAlreadyPresentResponse = 202;

        //Container related constants
        public static string BadContainerName = new string('a', 257);
        public static string BadContainerNameWithSlash = "thisIsAName/WithASlash";
        public static string BadContainerNameWithQuestionMark = "thisIsAName?WithAQuestionMark";
        public static string PublicContainerTTL = "10000";

        public const string EmptyContainerName = "";
        public const string QueryParameterPrefixValue = "test";
        public const string QueryParameterLimitValue = "0";
        public const string QueryParameterOffsetValue = "1";

        //StorageItem related constants
        public const string StorageItemName = "TestStorageItem.txt";
        public const string StorageItemNameGif = "TestStorageItem.gif";
        public const string StorageItemNameJpg = "TestStorageItem.jpg";
        public const string StorageItemNamePdf = "TestStorageItem.pdf";
        public const string HeadStorageItemName = "HeadStorageItem.txt";
        public const string EmptyStorageObjectName = "";
        public const string MetadataKey = "Testkey";
        public const string MetadataValue = "testvalue";
        public const string UploadFileName = "test.txt";
        public const string DownloadFileName = "testdownload.txt";
        public const int CustomChunkSize = 500;
        public const string BufferString = "Hello42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a";
        public const int SuccessPostStorageObject = 202;
        public const string NonExistingStorageObjectName = "FD8D79BF044E4a6eB157C701589C190BCE620C5A3D124285A6370004353CDD808F6CAE1229BE4be5812CC77AB8E98263";
        public const string EmptyFileName = "";
        public const string MaximumLenStorageObjectName = "42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077";
        public const string MaximumLenExceedStorageObjectName = "42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a";
        public const string MetadataKeyMaxLen = "42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077";
        public const string MetadataKeyExceedMaxLen = "42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a";
        public const string MetadataValueMaxLen = "42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077";
        public const string MetadataValueExceedMaxLen = "42F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D2507742F9A622ECA447d38ABD5581CA2DBEF0FC24075FBE684038BF0C366FD3D25077a";
        public const int ApiVersion = 1;

        public const string CloudVersion = "v1";
        public const int MaximumContainerNameLength = 256;

        public const string USER_AGENT = "csharp-cloudfiles";
    }
}