//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace com.mosso.cloudfiles.domain.request
{
    #region
    using System;
    using System.Collections.Generic;
    using System.IO;
    using com.mosso.cloudfiles.domain.request.Interfaces;
    using com.mosso.cloudfiles.exceptions;
    using com.mosso.cloudfiles.utils;
    #endregion

    /// <summary>
    /// A class to represent putting a storage item with a web request
    /// </summary>
    public class PutStorageItem : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly string _containerName;
        private readonly string _remoteStorageItemName;
        private readonly Stream fileToSend;
        private readonly Dictionary<string, string> _metadata;
        private readonly string _fileUrl;
        private Dictionary<string, string> _mimetypes;

        public event Connection.ProgressCallback Progress;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutStorageItem"/> class for putting an item without a file stream.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <param name="remoteStorageItemName">The name of the storage item to add meta information too</param>
        /// <param name="localFilePath">The path of the file to put into cloudfiles</param>
        public PutStorageItem(string storageUrl, string containerName, string remoteStorageItemName, string localFilePath)
            : this(storageUrl, containerName, remoteStorageItemName, localFilePath, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutStorageItem"/> class for putting an item without meta data.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <param name="remoteStorageItemName">The name of the storage item to add meta information too</param>
        /// <param name="filestream">The file stream of the file to put into cloudfiles</param>
        public PutStorageItem(string storageUrl, string containerName, string remoteStorageItemName, Stream filestream)
            : this(storageUrl, containerName, remoteStorageItemName, filestream, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutStorageItem"/> class for putting an item with meta data.
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="remoteStorageItemName">the name of the storage item to add meta information too</param>
        /// <param name="fileToSend">the file stream of the file to put into cloudfiles</param>
        /// <param name="metadata">dictionary of meta tags to apply to the storage item</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public PutStorageItem(string storageUrl, string containerName,
            string remoteStorageItemName,
            Stream fileToSend,
            Dictionary<string, string> metadata)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(containerName)
                || fileToSend == null
                || string.IsNullOrEmpty(remoteStorageItemName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }
            if (!ObjectNameValidator.Validate(remoteStorageItemName))
            {
                throw new StorageItemNameException();
            }

            _fileUrl = this.CleanUpFilePath(remoteStorageItemName);
            _storageUrl = storageUrl;
            _containerName = containerName;
            _remoteStorageItemName = remoteStorageItemName;
            _metadata = metadata;
            this.fileToSend = fileToSend;

            BuildMimeTypeDict();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutStorageItem"/> class for putting an item with a local file.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <param name="remoteStorageItemName">The name of the storage item to add meta information too</param>
        /// <param name="localFilePath">The path of the file to put into cloudfiles</param>
        /// <param name="metadata">Dictionary of meta tags to apply to the storage item</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public PutStorageItem(string storageUrl, string containerName, string remoteStorageItemName,
            string localFilePath,
            Dictionary<string, string> metadata)
        {
            _storageUrl = storageUrl;
            _containerName = containerName;
            _remoteStorageItemName = remoteStorageItemName;
            _metadata = metadata;

            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(containerName)
                || string.IsNullOrEmpty(localFilePath)
                || string.IsNullOrEmpty(remoteStorageItemName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }

            if (!ObjectNameValidator.Validate(remoteStorageItemName))
            {
                throw new StorageItemNameException();
            }

            _fileUrl = this.CleanUpFilePath(localFilePath);
            fileToSend = new FileStream(_fileUrl, FileMode.Open, FileAccess.Read);

            BuildMimeTypeDict();

        }

        /// <summary>
        /// Content type for the storage item
        /// </summary>
        /// <returns>
        /// Storage item's content type
        /// </returns>
        private string ContentType()
        {
            if (String.IsNullOrEmpty(_fileUrl) || _fileUrl.IndexOf(".") < 0)
            {
                return "application/octet-stream";
            }

            return MimeType(_fileUrl);
        }

        #region MIME Type Lookup Reference
        /// <summary>
        /// Builds the MIME type dictionary
        /// </summary>
        /// <remarks>TODO: Move to resource file</remarks>
        private void BuildMimeTypeDict()
        {
            _mimetypes = new Dictionary<string, string>
                             {
                                 {".3dm", "x-world/x-3dmf"},
                                 {".3dmf", "x-world/x-3dmf"},
                                 {".a", "application/octet-stream"},
                                 {".aab", "application/x-authorware-bin"},
                                 {".aam", "application/x-authorware-map"},
                                 {".aas", "application/x-authorware-seg"},
                                 {".abc", "text/vnd.abc"},
                                 {".acgi", "text/html"},
                                 {".afl", "video/animaflex"},
                                 {".ai", "application/postscript"},
                                 {".aif", "audio/aiff"},
                                 {".aifc", "audio/aiff"},
                                 {".aiff", "audio/aiff"},
                                 {".aim", "application/x-aim"},
                                 {".aip", "text/x-audiosoft-intra"},
                                 {".ani", "application/x-navi-animation"},
                                 {".aos", "application/x-nokia-9000-communicator-add-on-software"},
                                 {".aps", "application/mime"},
                                 {".arc", "application/octet-stream"},
                                 {".arj", "application/arj"},
                                 {".art", "image/x-jg"},
                                 {".asf", "video/x-ms-asf"},
                                 {".asm", "text/x-asm"},
                                 {".asp", "text/asp"},
                                 {".asx", "video/x-ms-asf"},
                                 {".au", "audio/basic"},
                                 {".avi", "video/avi"},
                                 {".avs", "video/avs-video"},
                                 {".bcpio", "application/x-bcpio"},
                                 {".bin", "application/octet-stream"},
                                 {".bm", "image/bmp"},
                                 {".bmp", "image/bmp"},
                                 {".boo", "application/book"},
                                 {".book", "application/book"},
                                 {".boz", "application/x-bzip2"},
                                 {".bsh", "application/x-bsh"},
                                 {".bz", "application/x-bzip"},
                                 {".bz2", "application/x-bzip2"},
                                 {".c", "text/plain"},
                                 {".c++", "text/plain"},
                                 {".cat", "application/vnd.ms-pki.seccat"},
                                 {".cc", "text/plain"},
                                 {".ccad", "application/clariscad"},
                                 {".cco", "application/x-cocoa"},
                                 {".cdf", "application/cdf"},
                                 {".cer", "application/pkix-cert"},
                                 {".cha", "application/x-chat"},
                                 {".chat", "application/x-chat"},
                                 {".class", "application/java"},
                                 {".com", "application/octet-stream"},
                                 {".conf", "text/plain"},
                                 {".cpio", "application/x-cpio"},
                                 {".cpp", "text/x-c"},
                                 {".cpt", "application/x-cpt"},
                                 {".crl", "application/pkcs-crl"},
                                 {".crt", "application/pkix-cert"},
                                 {".csh", "application/x-csh"},
                                 {".css", "text/css"},
                                 {".cxx", "text/plain"},
                                 {".dcr", "application/x-director"},
                                 {".deepv", "application/x-deepv"},
                                 {".def", "text/plain"},
                                 {".der", "application/x-x509-ca-cert"},
                                 {".dif", "video/x-dv"},
                                 {".dir", "application/x-director"},
                                 {".dl", "video/dl"},
                                 {".doc", "application/msword"},
                                 {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
                                 {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                                 {".dot", "application/msword"},
                                 {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
                                 {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
                                 {".dp", "application/commonground"},
                                 {".drw", "application/drafting"},
                                 {".dump", "application/octet-stream"},
                                 {".dv", "video/x-dv"},
                                 {".dvi", "application/x-dvi"},
                                 {".dwf", "model/vnd.dwf"},
                                 {".dwg", "image/vnd.dwg"},
                                 {".dxf", "image/vnd.dwg"},
                                 {".dxr", "application/x-director"},
                                 {".el", "text/x-script.elisp"},
                                 {".elc", "application/x-elc"},
                                 {".env", "application/x-envoy"},
                                 {".eps", "application/postscript"},
                                 {".es", "application/x-esrehber"},
                                 {".etx", "text/x-setext"},
                                 {".evy", "application/envoy"},
                                 {".exe", "application/octet-stream"},
                                 {".f", "text/plain"},
                                 {".f77", "text/x-fortran"},
                                 {".f90", "text/plain"},
                                 {".fdf", "application/vnd.fdf"},
                                 {".fif", "image/fif"},
                                 {".fli", "video/fli"},
                                 {".flo", "image/florian"},
                                 {".flx", "text/vnd.fmi.flexstor"},
                                 {".fmf", "video/x-atomic3d-feature"},
                                 {".for", "text/x-fortran"},
                                 {".fpx", "image/vnd.fpx"},
                                 {".frl", "application/freeloader"},
                                 {".funk", "audio/make"},
                                 {".g", "text/plain"},
                                 {".g3", "image/g3fax"},
                                 {".gif", "image/gif"},
                                 {".gl", "video/gl"},
                                 {".gsd", "audio/x-gsm"},
                                 {".gsm", "audio/x-gsm"},
                                 {".gsp", "application/x-gsp"},
                                 {".gss", "application/x-gss"},
                                 {".gtar", "application/x-gtar"},
                                 {".gz", "application/x-gzip"},
                                 {".gzip", "application/x-gzip"},
                                 {".h", "text/plain"},
                                 {".hdf", "application/x-hdf"},
                                 {".help", "application/x-helpfile"},
                                 {".hgl", "application/vnd.hp-hpgl"},
                                 {".hh", "text/plain"},
                                 {".hlb", "text/x-script"},
                                 {".hlp", "application/hlp"},
                                 {".hpg", "application/vnd.hp-hpgl"},
                                 {".hpgl", "application/vnd.hp-hpgl"},
                                 {".hqx", "application/binhex"},
                                 {".hta", "application/hta"},
                                 {".htc", "text/x-component"},
                                 {".htm", "text/html"},
                                 {".html", "text/html"},
                                 {".htmls", "text/html"},
                                 {".htt", "text/webviewhtml"},
                                 {".htx", "text/html"},
                                 {".ice", "x-conference/x-cooltalk"},
                                 {".ico", "image/x-icon"},
                                 {".idc", "text/plain"},
                                 {".ief", "image/ief"},
                                 {".iefs", "image/ief"},
                                 {".iges", "application/iges"},
                                 {".igs", "application/iges"},
                                 {".ima", "application/x-ima"},
                                 {".imap", "application/x-httpd-imap"},
                                 {".inf", "application/inf"},
                                 {".ins", "application/x-internett-signup"},
                                 {".ip", "application/x-ip2"},
                                 {".isu", "video/x-isvideo"},
                                 {".it", "audio/it"},
                                 {".iv", "application/x-inventor"},
                                 {".ivr", "i-world/i-vrml"},
                                 {".ivy", "application/x-livescreen"},
                                 {".jam", "audio/x-jam"},
                                 {".jav", "text/plain"},
                                 {".java", "text/plain"},
                                 {".jcm", "application/x-java-commerce"},
                                 {".jfif", "image/jpeg"},
                                 {".jfif-tbnl", "image/jpeg"},
                                 {".jpe", "image/jpeg"},
                                 {".jpeg", "image/jpeg"},
                                 {".jpg", "image/jpeg"},
                                 {".jps", "image/x-jps"},
                                 {".js", "application/x-javascript"},
                                 {".jut", "image/jutvision"},
                                 {".kar", "audio/midi"},
                                 {".ksh", "application/x-ksh"},
                                 {".la", "audio/nspaudio"},
                                 {".lam", "audio/x-liveaudio"},
                                 {".latex", "application/x-latex"},
                                 {".lha", "application/octet-stream"},
                                 {".lhx", "application/octet-stream"},
                                 {".list", "text/plain"},
                                 {".lma", "audio/nspaudio"},
                                 {".log", "text/plain"},
                                 {".lsp", "application/x-lisp"},
                                 {".lst", "text/plain"},
                                 {".lsx", "text/x-la-asf"},
                                 {".ltx", "application/x-latex"},
                                 {".lzh", "application/octet-stream"},
                                 {".lzx", "application/octet-stream"},
                                 {".m", "text/plain"},
                                 {".m1v", "video/mpeg"},
                                 {".m2a", "audio/mpeg"},
                                 {".m2v", "video/mpeg"},
                                 {".m3u", "audio/x-mpequrl"},
                                 {".man", "application/x-troff-man"},
                                 {".map", "application/x-navimap"},
                                 {".mar", "text/plain"},
                                 {".mbd", "application/mbedlet"},
                                 {".mc$", "application/x-magic-cap-package-1.0"},
                                 {".mcd", "application/mcad"},
                                 {".mcf", "text/mcf"},
                                 {".mcp", "application/netmc"},
                                 {".me", "application/x-troff-me"},
                                 {".mht", "message/rfc822"},
                                 {".mhtml", "message/rfc822"},
                                 {".mid", "audio/midi"},
                                 {".midi", "audio/midi"},
                                 {".mif", "application/x-mif"},
                                 {".mime", "message/rfc822"},
                                 {".mjf", "audio/x-vnd.audioexplosion.mjuicemediafile"},
                                 {".mjpg", "video/x-motion-jpeg"},
                                 {".mm", "application/base64"},
                                 {".mme", "application/base64"},
                                 {".mod", "audio/mod"},
                                 {".moov", "video/quicktime"},
                                 {".mov", "video/quicktime"},
                                 {".movie", "video/x-sgi-movie"},
                                 {".mp2", "audio/mpeg"},
                                 {".mp3", "audio/mpeg"},
                                 {".mpa", "audio/mpeg"},
                                 {".mpc", "application/x-project"},
                                 {".mpe", "video/mpeg"},
                                 {".mpeg", "video/mpeg"},
                                 {".mpg", "video/mpeg"},
                                 {".mpga", "audio/mpeg"},
                                 {".mpp", "application/vnd.ms-project"},
                                 {".mpt", "application/vnd.ms-project"},
                                 {".mpv", "application/vnd.ms-project"},
                                 {".mpx", "application/vnd.ms-project"},
                                 {".mrc", "application/marc"},
                                 {".ms", "application/x-troff-ms"},
                                 {".mv", "video/x-sgi-movie"},
                                 {".my", "audio/make"},
                                 {".mzz", "application/x-vnd.audioexplosion.mzz"},
                                 {".nap", "image/naplps"},
                                 {".naplps", "image/naplps"},
                                 {".nc", "application/x-netcdf"},
                                 {".ncm", "application/vnd.nokia.configuration-message"},
                                 {".nif", "image/x-niff"},
                                 {".niff", "image/x-niff"},
                                 {".nix", "application/x-mix-transfer"},
                                 {".nsc", "application/x-conference"},
                                 {".nvd", "application/x-navidoc"},
                                 {".o", "application/octet-stream"},
                                 {".oda", "application/oda"},
                                 {".omc", "application/x-omc"},
                                 {".omcd", "application/x-omcdatamaker"},
                                 {".omcr", "application/x-omcregerator"},
                                 {".p", "text/x-pascal"},
                                 {".p10", "application/pkcs10"},
                                 {".p12", "application/pkcs-12"},
                                 {".p7a", "application/x-pkcs7-signature"},
                                 {".p7c", "application/pkcs7-mime"},
                                 {".p7m", "application/pkcs7-mime"},
                                 {".p7r", "application/x-pkcs7-certreqresp"},
                                 {".p7s", "application/pkcs7-signature"},
                                 {".part", "application/pro_eng"},
                                 {".pas", "text/pascal"},
                                 {".pbm", "image/x-portable-bitmap"},
                                 {".pcl", "application/vnd.hp-pcl"},
                                 {".pct", "image/x-pict"},
                                 {".pcx", "image/x-pcx"},
                                 {".pdb", "chemical/x-pdb"},
                                 {".pdf", "application/pdf"},
                                 {".pfunk", "audio/make"},
                                 {".pgm", "image/x-portable-greymap"},
                                 {".pic", "image/pict"},
                                 {".pict", "image/pict"},
                                 {".pkg", "application/x-newton-compatible-pkg"},
                                 {".pko", "application/vnd.ms-pki.pko"},
                                 {".pl", "text/plain"},
                                 {".plx", "application/x-pixclscript"},
                                 {".pm", "image/x-xpixmap"},
                                 {".pm4", "application/x-pagemaker"},
                                 {".pm5", "application/x-pagemaker"},
                                 {".png", "image/png"},
                                 {".pnm", "application/x-portable-anymap"},
                                 {".pot", "application/vnd.ms-powerpoint"},
                                 {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
                                 {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
                                 {".pov", "model/x-pov"},
                                 {".ppa", "application/vnd.ms-powerpoint"},
                                 {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
                                 {".ppm", "image/x-portable-pixmap"},
                                 {".pps", "application/vnd.ms-powerpoint"},
                                 {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
                                 {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
                                 {".ppt", "application/vnd.ms-powerpoint"},
                                 {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
                                 {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                                 {".ppz", "application/vnd.ms-powerpoint"},
                                 {".pre", "application/x-freelance"},
                                 {".prt", "application/pro_eng"},
                                 {".ps", "application/postscript"},
                                 {".psd", "application/octet-stream"},
                                 {".pvu", "paleovu/x-pv"},
                                 {".pwz", "application/vnd.ms-powerpoint"},
                                 {".py", "text/x-script.phyton"},
                                 {".pyc", "applicaiton/x-bytecode.python"},
                                 {".qcp", "audio/vnd.qcelp"},
                                 {".qd3", "x-world/x-3dmf"},
                                 {".qd3d", "x-world/x-3dmf"},
                                 {".qif", "image/x-quicktime"},
                                 {".qt", "video/quicktime"},
                                 {".qtc", "video/x-qtc"},
                                 {".qti", "image/x-quicktime"},
                                 {".qtif", "image/x-quicktime"},
                                 {".ra", "audio/x-pn-realaudio"},
                                 {".ram", "audio/x-pn-realaudio"},
                                 {".ras", "application/x-cmu-raster"},
                                 {".rast", "image/cmu-raster"},
                                 {".rexx", "text/x-script.rexx"},
                                 {".rf", "image/vnd.rn-realflash"},
                                 {".rgb", "image/x-rgb"},
                                 {".rm", "application/vnd.rn-realmedia"},
                                 {".rmi", "audio/mid"},
                                 {".rmm", "audio/x-pn-realaudio"},
                                 {".rmp", "audio/x-pn-realaudio"},
                                 {".rng", "application/ringing-tones"},
                                 {".rnx", "application/vnd.rn-realplayer"},
                                 {".roff", "application/x-troff"},
                                 {".rp", "image/vnd.rn-realpix"},
                                 {".rpm", "audio/x-pn-realaudio-plugin"},
                                 {".rt", "text/richtext"},
                                 {".rtf", "text/richtext"},
                                 {".rtx", "text/richtext"},
                                 {".rv", "video/vnd.rn-realvideo"},
                                 {".s", "text/x-asm"},
                                 {".s3m", "audio/s3m"},
                                 {".saveme", "application/octet-stream"},
                                 {".sbk", "application/x-tbook"},
                                 {".scm", "application/x-lotusscreencam"},
                                 {".sdml", "text/plain"},
                                 {".sdp", "application/sdp"},
                                 {".sdr", "application/sounder"},
                                 {".sea", "application/sea"},
                                 {".set", "application/set"},
                                 {".sgm", "text/sgml"},
                                 {".sgml", "text/sgml"},
                                 {".sh", "application/x-sh"},
                                 {".shar", "application/x-shar"},
                                 {".shtml", "text/html"},
                                 {".sid", "audio/x-psid"},
                                 {".sit", "application/x-sit"},
                                 {".skd", "application/x-koan"},
                                 {".skm", "application/x-koan"},
                                 {".skp", "application/x-koan"},
                                 {".skt", "application/x-koan"},
                                 {".sl", "application/x-seelogo"},
                                 {".smi", "application/smil"},
                                 {".smil", "application/smil"},
                                 {".snd", "audio/basic"},
                                 {".sol", "application/solids"},
                                 {".spc", "text/x-speech"},
                                 {".spl", "application/futuresplash"},
                                 {".spr", "application/x-sprite"},
                                 {".sprite", "application/x-sprite"},
                                 {".src", "application/x-wais-source"},
                                 {".ssi", "text/x-server-parsed-html"},
                                 {".ssm", "application/streamingmedia"},
                                 {".sst", "application/vnd.ms-pki.certstore"},
                                 {".step", "application/step"},
                                 {".stl", "application/sla"},
                                 {".stp", "application/step"},
                                 {".sv4cpio", "application/x-sv4cpio"},
                                 {".sv4crc", "application/x-sv4crc"},
                                 {".svf", "image/vnd.dwg"},
                                 {".svr", "application/x-world"},
                                 {".swf", "application/x-shockwave-flash"},
                                 {".t", "application/x-troff"},
                                 {".talk", "text/x-speech"},
                                 {".tar", "application/x-tar"},
                                 {".tbk", "application/toolbook"},
                                 {".tcl", "application/x-tcl"},
                                 {".tcsh", "text/x-script.tcsh"},
                                 {".tex", "application/x-tex"},
                                 {".texi", "application/x-texinfo"},
                                 {".texinfo", "application/x-texinfo"},
                                 {".text", "text/plain"},
                                 {".tgz", "application/x-compressed"},
                                 {".tif", "image/tiff"},
                                 {".tiff", "image/tiff"},
                                 {".tr", "application/x-troff"},
                                 {".tsi", "audio/tsp-audio"},
                                 {".tsp", "application/dsptype"},
                                 {".tsv", "text/tab-separated-values"},
                                 {".turbot", "image/florian"},
                                 {".txt", "text/plain"},
                                 {".uil", "text/x-uil"},
                                 {".uni", "text/uri-list"},
                                 {".unis", "text/uri-list"},
                                 {".unv", "application/i-deas"},
                                 {".uri", "text/uri-list"},
                                 {".uris", "text/uri-list"},
                                 {".ustar", "application/x-ustar"},
                                 {".uu", "application/octet-stream"},
                                 {".uue", "text/x-uuencode"},
                                 {".vcd", "application/x-cdlink"},
                                 {".vcs", "text/x-vcalendar"},
                                 {".vda", "application/vda"},
                                 {".vdo", "video/vdo"},
                                 {".vew", "application/groupwise"},
                                 {".viv", "video/vivo"},
                                 {".vivo", "video/vivo"},
                                 {".vmd", "application/vocaltec-media-desc"},
                                 {".vmf", "application/vocaltec-media-file"},
                                 {".voc", "audio/voc"},
                                 {".vos", "video/vosaic"},
                                 {".vox", "audio/voxware"},
                                 {".vqe", "audio/x-twinvq-plugin"},
                                 {".vqf", "audio/x-twinvq"},
                                 {".vql", "audio/x-twinvq-plugin"},
                                 {".vrml", "application/x-vrml"},
                                 {".vrt", "x-world/x-vrt"},
                                 {".vsd", "application/x-visio"},
                                 {".vst", "application/x-visio"},
                                 {".vsw", "application/x-visio"},
                                 {".w60", "application/wordperfect6.0"},
                                 {".w61", "application/wordperfect6.1"},
                                 {".w6w", "application/msword"},
                                 {".wav", "audio/wav"},
                                 {".wb1", "application/x-qpro"},
                                 {".wbmp", "image/vnd.wap.wbmp"},
                                 {".web", "application/vnd.xara"},
                                 {".wiz", "application/msword"},
                                 {".wk1", "application/x-123"},
                                 {".wmf", "windows/metafile"},
                                 {".wml", "text/vnd.wap.wml"},
                                 {".wmlc", "application/vnd.wap.wmlc"},
                                 {".wmls", "text/vnd.wap.wmlscript"},
                                 {".wmlsc", "application/vnd.wap.wmlscriptc"},
                                 {".word", "application/msword"},
                                 {".wp", "application/wordperfect"},
                                 {".wp5", "application/wordperfect"},
                                 {".wp6", "application/wordperfect"},
                                 {".wpd", "application/wordperfect"},
                                 {".wq1", "application/x-lotus"},
                                 {".wri", "application/mswrite"},
                                 {".wrl", "application/x-world"},
                                 {".wrz", "x-world/x-vrml"},
                                 {".wsc", "text/scriplet"},
                                 {".wsrc", "application/x-wais-source"},
                                 {".wtk", "application/x-wintalk"},
                                 {".xbm", "image/x-xbitmap"},
                                 {".xdr", "video/x-amt-demorun"},
                                 {".xgz", "xgl/drawing"},
                                 {".xif", "image/vnd.xiff"},
                                 {".xl", "application/excel"},
                                 {".xla", "application/vnd.ms-excel"},
                                 {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
                                 {".xlb", "application/vnd.ms-excel"},
                                 {".xlc", "application/vnd.ms-excel"},
                                 {".xld", "application/vnd.ms-excel"},
                                 {".xlk", "application/vnd.ms-excel"},
                                 {".xll", "application/vnd.ms-excel"},
                                 {".xlm", "application/vnd.ms-excel"},
                                 {".xls", "application/vnd.ms-excel"},
                                 {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
                                 {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
                                 {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                                 {".xlt", "application/vnd.ms-excel"},
                                 {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
                                 {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
                                 {".xlv", "application/vnd.ms-excel"},
                                 {".xlw", "application/vnd.ms-excel"},
                                 {".xm", "audio/xm"},
                                 {".xml", "application/xml"},
                                 {".xmz", "xgl/movie"},
                                 {".xpix", "application/x-vnd.ls-xpix"},
                                 {".xpm", "image/xpm"},
                                 {".x-png", "image/png"},
                                 {".xsr", "video/x-amt-showrun"},
                                 {".xwd", "image/x-xwd"},
                                 {".xyz", "chemical/x-pdb"},
                                 {".z", "application/x-compressed"},
                                 {".zip", "application/zip"},
                                 {".zoo", "application/octet-stream"},
                                 {".zsh", "text/x-script.zsh"}
                             };
        }
        #endregion

        /// <summary>
        /// Retrieve the MIME type for a specified filename based on extension.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The MIME type</returns>
        private string MimeType(string filename)
        {
            var extension = Path.GetExtension(filename).ToLower();

            string mimetype;
            _mimetypes.TryGetValue(extension, out mimetype);

            return mimetype ?? "application/octet-stream";
        }

        /// <summary>
        /// Cleans up file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Cleaned file path</returns>
        private string CleanUpFilePath(string filePath)
        {
            return filePath.StripSlashPrefix().Replace(@"file:\\\", "");
        }

        /// <summary>
        /// Creates the corresponding URI for this request using the storage item info.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return new Uri(_storageUrl + "/" + _containerName.Encode() + "/" + _remoteStorageItemName.StripSlashPrefix().Encode());
        }

        /// <summary>
        /// Applies the appropiate properties to the specified request for this implementation.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "PUT";

            if (_metadata != null && _metadata.Count > 0)
            {
                foreach (var s in _metadata.Keys)
                {
                    request.Headers.Add(Constants.META_DATA_HEADER + s, _metadata[s]);
                }
            }

            request.AllowWriteStreamBuffering = false;
            request.ContentType = ContentType();
            request.SetContent(fileToSend, Progress);
        }

    }
}