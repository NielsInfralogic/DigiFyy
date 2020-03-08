using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using MySql.Data.MySqlClient;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSLambda
{
    public class Function
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public List<FrameNumbers> RegisterUIDHandler(string uid, ILambdaContext context)
        {
            List<FrameNumbers> frameNumbers = new List<FrameNumbers>();

            try
            {
                MySqlConnection connection = new MySqlConnection
                {
                    ConnectionString = "server=database-1-instance-1.cjj4nkdwan9g.eu-north-1.rds.amazonaws.com;user id=admin;password=Infra2Logic;port=3306;database=Digifyy;"
                };
                string sql = "SELECT Digifyy.FrameNumbers.UID, Digifyy.Manufacturers.Name, Digifyy.FrameNumbers.Model, Digifyy.FrameNumbers.Frame,Digifyy.Owners.Name, Digifyy.FrameNumbers.LastUpdateTime, Digifyy.FrameNumbers.LastUpdateClientID " +
                            "FROM Digifyy.FrameNumbers " +
                            "INNER JOIN Digifyy.Manufacturers ON Digifyy.Manufacturers.ManufacturerID = Digifyy.FrameNumbers.ManufacturerID " +
                            "INNER JOIN Digifyy.Owners ON Digifyy.Owners.UID = Digifyy.FrameNumbers.UID;";
                if (string.IsNullOrEmpty(uid) == false)
                    sql += $" WHERE Digifyy.FrameNumbers.UID='{uid}";

                MySqlCommand command = new MySqlCommand(sql, connection)
                {
                    CommandTimeout = 45 // Seconds
                };
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        frameNumbers.Add(new FrameNumbers()
                        {
                            UID = reader.GetString(0),
                            Manufacturer = reader.GetString(1),
                            Model = reader.GetString(2),
                            Frame = reader.GetString(3),
                            Owner = reader.GetString(4),
                            LastUpdateTime = reader.GetDateTime(5),
                            LastUpdateClientID = reader.GetString(6)
                        });
                    }

                    reader.Close();

                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                return frameNumbers;
            }

            return frameNumbers;
        }


    }
}
