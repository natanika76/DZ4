using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Data;
using System.Linq.Expressions;

namespace DZ1
{
    internal class Program
    {
        static async Task MainAsync(string[] args)
        {
            /*
         string connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=Fruits; Integrated Security=SSPI;";

         //Вставляем фрукты и овощи
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
             string insertQuery = "INSERT INTO VegetablesAndFruits (Name, Type, Color, Calories) VALUES (@Name, @Type, @Color, @Calories)";

             try
             {
                 connection.Open();

                 // Добавление записи о яблоке
                 SqlCommand command1 = new SqlCommand(insertQuery, connection);
                 command1.Parameters.AddWithValue("@Name", "Яблоко");
                 command1.Parameters.AddWithValue("@Type", "Фрукт");
                 command1.Parameters.AddWithValue("@Color", "Красный");
                 command1.Parameters.AddWithValue("@Calories", 52);
                 int rowsAffected1 = command1.ExecuteNonQuery();
                 Console.WriteLine($"Добавлено {rowsAffected1} записей.");

                 // Добавление записи о банане
                 SqlCommand command2 = new SqlCommand(insertQuery, connection);
                 command2.Parameters.AddWithValue("@Name", "Банан");
                 command2.Parameters.AddWithValue("@Type", "Фрукт");
                 command2.Parameters.AddWithValue("@Color", "Желтый");
                 command2.Parameters.AddWithValue("@Calories", 89);
                 int rowsAffected2 = command2.ExecuteNonQuery();
                 Console.WriteLine($"Добавлено {rowsAffected2} записей.");

                 // Добавление записи о моркови
                 SqlCommand command3 = new SqlCommand(insertQuery, connection);
                 command3.Parameters.AddWithValue("@Name", "Морковь");
                 command3.Parameters.AddWithValue("@Type", "Овощ");
                 command3.Parameters.AddWithValue("@Color", "Оранжевый");
                 command3.Parameters.AddWithValue("@Calories", 41);
                 int rowsAffected3 = command3.ExecuteNonQuery();
                 Console.WriteLine($"Добавлено {rowsAffected3} записей.");

             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Ошибка: {ex.Message}");
             }
         }*/
            //-----------------------------------------------------------------------
            
            
                string connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=Fruits; Integrated Security=True;";

                Console.WriteLine("Попытка подключения к базе данных Fruits...");
                await Task.Run(async () =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            await connection.OpenAsync();

                            Console.WriteLine("Подключение успешно установлено!");
                            Console.WriteLine($"Строка подключения: {connection.ConnectionString}");

                          
                            await ShowAllDataAsync(connection);
                            await ShowVegetableCountAsync(connection);
                            await ShowFruitCountAsync(connection);
                            await ShowItemCountByColorAsync(connection, "Красный");
                            await ShowItemCountByColorAsync(connection);

                            await ShowItemsBelowCaloriesAsync(connection, 50);
                            await ShowItemsAboveCaloriesAsync(connection, 50);
                            await ShowItemsInCaloriesRangeAsync(connection, 35, 55);
                            await ShowItemsByColorAsync(connection, "Красный", "Желтый");

                            await ShowAllNamesAsync(connection);
                            await ShowAllColorsAsync(connection);
                            await ShowMaxCaloriesAsync(connection);
                            await ShowMinCaloriesAsync(connection);
                            await ShowAvgCaloriesAsync(connection);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при подключении к базе данных: {ex.Message}");
                            return;
                        }

                        Console.WriteLine("Нажмите Enter для отключения от базы данных и выхода из приложения...");
                        Console.ReadLine();
                    }

                    Console.WriteLine("Отключение от базы данных...");
                    Console.WriteLine("Для выхода из приложения нажмите Enter...");
                    Console.ReadLine();

                });
           
        }
    }

    static async Task ShowAllDataAsync(SqlConnection connection)
        {
        DateTime startTime = DateTime.Now; // Фиксируем время начала выполнения запроса

        Console.WriteLine("\nВся информация из таблицы с овощами и фруктами:");

            string query = "SELECT * FROM VegetablesAndFruits";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = await command.ExecuteReader();
            while (await reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["Type"]}, {reader["Color"]}, {reader["Calories"]} калорий");
            }
            reader.Close();

        DateTime endTime = DateTime.Now; // Фиксируем время завершения выполнения запроса

        TimeSpan executionTime = endTime - startTime; // Вычисляем время выполнения запроса
        Console.WriteLine($"Время выполнения запроса: {executionTime.TotalSeconds} секунд"); // Выводим время выполнения запроса
    }

        static async Task ShowAllNamesAsync(SqlConnection connection)
        {
            Console.WriteLine("\nВсе названия овощей и фруктов:");

            string query = "SELECT DISTINCT Name FROM VegetablesAndFruits";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = await command.ExecuteReader();
            while (await reader.Read())
            {
                Console.WriteLine(reader["Name"]);
            }
            reader.Close();
        }

        static async Task ShowAllColorsAsync(SqlConnection connection)
        {
            Console.WriteLine("\nВсе цвета:");

            string query = "SELECT DISTINCT Color FROM VegetablesAndFruits";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = await command.ExecuteReader();
            while (await reader.Read())
            {
                Console.WriteLine(reader["Color"]);
            }
            reader.Close();
        }
        static async Task ShowMaxCaloriesAsync(SqlConnection connection)
        {
            Console.WriteLine("\nМаксимальная калорийность:");

            string query = "SELECT MAX(Calories) FROM VegetablesAndFruits";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = await command.ExecuteReader();
            if (await reader.Read())
            {
                int maxCalories = reader.GetInt32(0);
                Console.WriteLine(maxCalories + " калорий");
            }
            reader.Close();
        }
        static async Task ShowMinCaloriesAsync(SqlConnection connection)
        {
            Console.WriteLine("\nМинимальная калорийность:");

            string query = "SELECT MIN(Calories) FROM VegetablesAndFruits";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = await command.ExecuteReader();
            if (await reader.Read())
            {
                int minCalories = reader.GetInt32(0);
                Console.WriteLine(minCalories + " калорий");
            }
            reader.Close();
        }
       
        static async Task ShowAvgCaloriesAsync(SqlConnection connection)
        {
            Console.WriteLine("\nСредняя калорийность:");

            string query = "SELECT AVG(Calories) FROM VegetablesAndFruits";
            SqlCommand command = new SqlCommand(query, connection);

            object result = await command.ExecuteScalar();
            
            if (result != DBNull.Value)
            {
                double avgCalories = Convert.ToDouble(result);
                Console.WriteLine(avgCalories.ToString("F2") + " калорий");
            }
        }
        static async Task ShowVegetableCountAsync(SqlConnection connection)
        {
            Console.WriteLine("\nКоличество овощей:");

            string query = "SELECT COUNT(*) FROM VegetablesAndFruits WHERE Type=N'Овощ'";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = await command.ExecuteReader();
            if (await reader.Read())
            {
                int vegetableCount = reader.GetInt32(0);
                Console.WriteLine(vegetableCount + " овощей");
            }
            reader.Close();
        }
        static async Task ShowFruitCountAsync(SqlConnection connection)
        {
            string query = "SELECT COUNT(*) FROM VegetablesAndFruits WHERE Type = N'Фрукт'";
            SqlCommand command = new SqlCommand(query, connection);

            int fruitCount = await (int)command.ExecuteScalar();
            Console.WriteLine($"Количество фруктов: {fruitCount}");
        }
        static async Task ShowItemCountByColorAsync(SqlConnection connection, string color)
        {
            string query = "SELECT COUNT(*) FROM VegetablesAndFruits WHERE Color = @Color";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Color", color);

            int itemCount = await (int)command.ExecuteScalar();
            Console.WriteLine($"Количество овощей и фруктов цвета {color}: {itemCount}");
        }
        static async Task ShowItemCountByColorAsync(SqlConnection connection)
        {
            string query = "SELECT Color, COUNT(*) FROM VegetablesAndFruits GROUP BY Color";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = await command.ExecuteReader();
            while (await reader.Read())
            {
                string color = reader.GetString(0);
                int itemCount = reader.GetInt32(1);
                Console.WriteLine($"Количество овощей и фруктов цвета {color}: {itemCount}");
            }
            reader.Close();
        }
        static async Task ShowItemsBelowCaloriesAsync(SqlConnection connection, int calories)
        {
            string query = "SELECT Name, Type, Color, Calories FROM VegetablesAndFruits WHERE Calories < @Calories";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Calories", calories);

            SqlDataReader reader = await command.ExecuteReader();
            Console.WriteLine($"Овощи и фрукты с калорийностью ниже {calories}:");
            while (await reader.Read())
            {
                string name = reader.GetString(0);
                string type = reader.GetString(1);
                string color = reader.GetString(2);
                int itemCalories = reader.GetInt32(3);
                Console.WriteLine($"- {name} ({type}), цвет: {color}, калорийность: {itemCalories}");
            }
            reader.Close();
        }
        static async Task ShowItemsAboveCaloriesAsync(SqlConnection connection, int calories)
        {
            string query = "SELECT Name, Type, Color, Calories FROM VegetablesAndFruits WHERE Calories > @Calories";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Calories", calories);

            SqlDataReader reader = await command.ExecuteReader();
            Console.WriteLine($"Овощи и фрукты с калорийностью выше {calories}:");
            while (await reader.Read())
            {
                string name = reader.GetString(0);
                string type = reader.GetString(1);
                string color = reader.GetString(2);
                int itemCalories = reader.GetInt32(3);
                Console.WriteLine($"- {name} ({type}), цвет: {color}, калорийность: {itemCalories}");
            }
            reader.Close();
        }
        static async Task ShowItemsInCaloriesRangeAsync(SqlConnection connection, int minCalories, int maxCalories)
        {
            string query = "SELECT Name, Type, Color, Calories FROM VegetablesAndFruits WHERE Calories BETWEEN @MinCalories AND @MaxCalories";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MinCalories", minCalories);
            command.Parameters.AddWithValue("@MaxCalories", maxCalories);

            SqlDataReader reader = await command.ExecuteReader();
            Console.WriteLine($"Овощи и фрукты с калорийностью в диапазоне от {minCalories} до {maxCalories}:");
            while (await reader.Read())
            {
                string name = reader.GetString(0);
                string type = reader.GetString(1);
                string color = reader.GetString(2);
                int itemCalories = reader.GetInt32(3);
                Console.WriteLine($"- {name} ({type}), цвет: {color}, калорийность: {itemCalories}");
            }
            reader.Close();
        }
        static async Task ShowItemsByColorAsync(SqlConnection connection, params string[] colors)
        {
            // Создаем список параметров для цветов
            List<string> parameters = new List<string>();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            for (int i = 0; i < colors.Length; i++)
            {
                string paramName = $"@Color{i}";
                parameters.Add(paramName);
                sqlParameters.Add(new SqlParameter(paramName, SqlDbType.NVarChar) { Value = colors[i] });
            }

            // Создаем условие WHERE с использованием параметров
            string colorConditions = string.Join(" OR ", parameters.Select(p => $"Color = {p}"));

            // Создаем SQL-запрос с использованием параметризованного условия WHERE
            string query = $"SELECT Name, Type, Color, Calories FROM VegetablesAndFruits WHERE {colorConditions}";

            // Создаем команду с использованием SQL-запроса и параметров
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddRange(sqlParameters.ToArray());

            // Выполняем запрос и выводим результат
            using (SqlDataReader reader = await command.ExecuteReader())
            {
                Console.WriteLine($"Овощи и фрукты с цветом {string.Join(" или ", colors)}:");
                while (await reader.Read())
                {
                    string name = reader.GetString(0);
                    string type = reader.GetString(1);
                    string color = reader.GetString(2);
                    int itemCalories = reader.GetInt32(3);
                    Console.WriteLine($"- {name} ({type}), цвет: {color}, калорийность: {itemCalories}");
                }
            }
        }
    }
}
