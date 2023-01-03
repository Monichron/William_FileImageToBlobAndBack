using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FileImageToBlobAndBlobBackToImage
{
    public class Images
    {
        public void ToBlobFromFile()
        {
            using (SqlConnection connection = new SqlConnection("Server=DESKTOP-V339TRT;Database=LigmaBallsTestDb;Trusted_Connection=True;"))
            {
                connection.Open();

                // Create a command to insert the image into the database
                string insertSql = "INSERT INTO [images] (id, [data]) VALUES (@id, @data)";
                SqlCommand command = new SqlCommand(insertSql, connection);
                command.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                command.Parameters.Add("@data", SqlDbType.VarBinary);

                // Get a list of all image files in the specified directory
                string[] imageFiles = Directory.GetFiles("C:\\Users\\E7470\\Pictures\\Saved Pictures", "*.png");

                // Loop through each image file
                foreach (string imageFile in imageFiles)
                {
                    // Read the image data into a byte array
                    byte[] imageData = File.ReadAllBytes(imageFile);

                    // Set the ID and data parameters for the insert command
                    command.Parameters["@id"].Value = Guid.NewGuid(); ;
                    command.Parameters["@data"].Value = imageData;

                    // Execute the insert command
                    command.ExecuteNonQuery();
                }
            }
        }
        public void FromBlobToImage()
        {
            SqlCommand cmd = new SqlCommand("Select * from Images");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow item in dt.Rows)
            {
                string base64string = $"{item["data"]}";  // Put the full string here
                byte[] blob = Convert.FromBase64String(base64string);
                using (MemoryStream ms = new MemoryStream(blob))
                {
                    Image image = Image.FromStream(ms);
                }
            }
        }
    }
}

