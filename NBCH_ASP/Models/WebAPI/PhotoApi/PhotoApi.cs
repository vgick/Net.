
namespace NBCH_ASP.Models.WebAPI.PhotoApi {
    /// <summary>
    /// Информация о загруженном файле.
    /// </summary>
    public struct UploadedPhoto {
        /// <summary>
        /// ID файла
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Дата загрузки
        /// </summary>
        public string UploadDate { get; set; }
    }
}