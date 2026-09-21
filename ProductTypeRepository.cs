using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace WindowsFormsApp1
{
    public class ProductTypeRepository
    {
        public List<ProductType> GetAll()
        {
            List<ProductType> list = new List<ProductType>();
            string sql = "SELECT id_pt, name_pt FROM product_type ORDER BY name_pt";

            using (SqlConnection conn = DbHelper.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductType t = new ProductType();
                        t.Id = reader.GetInt32(reader.GetOrdinal("id_pt"));
                        t.Name = reader.GetString(reader.GetOrdinal("name_pt"));
                        list.Add(t);
                    }
                }
            }
            return list;
        }
    }
}