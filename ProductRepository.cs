using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace WindowsFormsApp1
{
    public class ProductRepository
    {
        private const string BaseSelect =
            "SELECT p.id_pd, p.name_pd, p.price_pd, " +
            "p.detail_pd, p.id_pt, t.name_pt " +
            "FROM products p " +
            "INNER JOIN product_type t ON p.id_pt = t.id_pt";

        private static Product Map(SqlDataReader reader)
        {
            Product product = new Product();
            product.Id = reader.GetInt32(reader.GetOrdinal("id_pd"));
            product.Name = reader.GetString(reader.GetOrdinal("name_pd"));
            product.Price = reader.GetDouble(reader.GetOrdinal("price_pd"));
            product.Detail = reader.GetString(reader.GetOrdinal("detail_pd"));
            product.TypeId = reader.GetInt32(reader.GetOrdinal("id_pt"));
            product.TypeName = reader.GetString(reader.GetOrdinal("name_pt"));
            return product;
        }

        public List<Product> Search(string keyword)
        {
            List<Product> list = new List<Product>();
            string sql = BaseSelect;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " WHERE p.name_pd LIKE @keyword";
            }
            sql += " ORDER BY p.id_pd";

            using (SqlConnection conn = DbHelper.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword.Trim() + "%");
                }

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(Map(reader));
                    }
                }
            }
            return list;
        }

        public Product SearchById(int id)
        {
            string sql = BaseSelect + " WHERE p.id_pd = @id";

            using (SqlConnection conn = DbHelper.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Map(reader);
                    }
                }
            }
            return null;
        }

        public int Insert(Product product)
        {
            string sql = @"
                INSERT INTO products (name_pd, price_pd, detail_pd, id_pt)
                VALUES (@name, @price, @detail, @typeId);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection conn = DbHelper.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@detail", product.Detail);
                cmd.Parameters.AddWithValue("@typeId", product.TypeId);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int Update(Product product)
        {
            string sql = @"
                UPDATE products
                SET name_pd   = @name,
                    price_pd  = @price,
                    detail_pd = @detail,
                    id_pt     = @typeId
                WHERE id_pd = @id";

            using (SqlConnection conn = DbHelper.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@detail", product.Detail);
                cmd.Parameters.AddWithValue("@typeId", product.TypeId);
                cmd.Parameters.AddWithValue("@id", product.Id);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Delete(int id)
        {
            string sql = "DELETE FROM products WHERE id_pd = @id";

            using (SqlConnection conn = DbHelper.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}