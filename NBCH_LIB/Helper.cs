using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace NBCH_LIB {
	public static class Helper {

		/// <summary>
		/// Часовой пояс сервера
		/// </summary>
		/// <returns>Часовой пояс сервера</returns>
		public static int ServerTimeZone {
			get {
				TimeZoneInfo localZone = TimeZoneInfo.Local;
				return Math.Abs(localZone.BaseUtcOffset.Hours);
			}
		}

		// Получить конец дня
		public static DateTime EndOfDay(DateTime date) {
			return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
		}

		// Получить начало дня
		public static DateTime BeginOfDay(DateTime date) {
			return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
		}

		/// <summary>
		/// Расширяющий метод, возвращающий описание перечисления
		/// </summary>
		/// <param name="enumElement"></param>
		/// <returns>Описание перечисления</returns>
		public static string GetDescription(this Enum enumElement) {
			Type type	= enumElement.GetType();

			MemberInfo[] memInfo	= type.GetMember(enumElement.ToString());
			if (memInfo.Length > 0) {
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (attrs.Length > 0)
					return ((DescriptionAttribute)attrs[0]).Description;
			}

			return enumElement.ToString();
		}

		/// <summary>
		/// Получить значение перечисления для отображения
		/// </summary>
		/// <param name="enumElement"></param>
		/// <returns>Значение для отображения перечисления</returns>
		public static string GetDisplayName(this Enum enumElement) {
			Type type			= enumElement.GetType();
			MemberInfo[] member	= type.GetMember(enumElement.ToString());
			DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

			if (displayName != null) {
				return displayName.Name;
			}

			return enumElement.ToString();
		}

		/// <summary>
		/// Записать на диск XML файл
		/// </summary>
		/// <param name="fileName">Имя файла</param>
		/// <param name="content">Данные XML файла</param>
		public static void SaveToFile(string fileName, byte[] content) {
			MemoryStream memoryStream	= new MemoryStream(content);
			memoryStream.Seek(0, SeekOrigin.Begin);

			FileStream fileStream	= new FileStream(fileName, FileMode.OpenOrCreate);
			memoryStream.WriteTo(fileStream);
			fileStream.Close();
		}


	}
}
