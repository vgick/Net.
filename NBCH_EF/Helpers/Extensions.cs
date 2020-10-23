using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using NBCH_EF.Tables;
using NBCH_LIB.Logger;

namespace NBCH_EF.Helpers {
	internal static class Extensions {
		/// <summary>
		/// Сохранить изменения в БД асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="context">Расширяемый класс (DBContext)</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		internal static async Task<int> SaveChangesAndLogErrorAsync<TLoggedClass>(this IDBSource context,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken) where TLoggedClass : class {
			
			return await ContextLogErrorAsync(async () => await context.SaveChangesAsync(cancellationToken), loggedMessage);
		}

		/// <summary>
		/// Сохранить изменения в БД асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="context">Расширяемый класс (DBContext)</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		internal static async Task<int> SaveChangesAndLogErrorAsync<TLoggedClass>(this IDBSource context,
			CancellationToken cancellationToken) where TLoggedClass : class {
			
			ILogger<TLoggedClass> logger				= MKKContext.LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage	=
				new LoggedMessage<TLoggedClass>(logger, "Ошибка внесения изменений в БД.");

			return await context.SaveChangesAndLogErrorAsync<TLoggedClass>(loggedMessage, cancellationToken);
		}

		/// <summary>
		/// Сохранить изменения в БД асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="context">Расширяемый класс (DBContext)</param>
		/// <param name="logShortMessage">Сообщение для логгера</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		internal static async Task<int> SaveChangesAndLogErrorAsync<TLoggedClass>(this IDBSource context,
			LogShortMessage logShortMessage, CancellationToken cancellationToken) where TLoggedClass : class {
			
			ILogger<TLoggedClass> logger = MKKContext.LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage =
				new LoggedMessage<TLoggedClass>(logger, logShortMessage.Message, logShortMessage.Params);

			return await context.SaveChangesAndLogErrorAsync(loggedMessage, cancellationToken);
		}

		/// <summary>
		/// Получить список объектов асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TResult">Список объектов</typeparam>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список объектов</returns>
		internal static async Task<List<TResult>> ToListAndLogErrorAsync<TResult, TLoggedClass>(this IQueryable<TResult> query,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken) where TLoggedClass : class {
			
			return await ContextLogErrorAsync(async () => await query.ToListAsync(cancellationToken), loggedMessage);
		}

		/// <summary>
		/// Получить список объектов асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TResult">Список объектов</typeparam>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список объектов</returns>
		internal static async Task<List<TResult>> ToListAndLogErrorAsync<TResult, TLoggedClass>(this IQueryable<TResult> query,
			CancellationToken cancellationToken) where TLoggedClass : class {
			
			ILogger<TLoggedClass> logger				= MKKContext.LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage	=
				new LoggedMessage<TLoggedClass>(logger, "Ошибка получения списка из БД./ Объект {TResult}.*/", typeof(TResult));

			return await ContextLogErrorAsync(async () => await query.ToListAsync(cancellationToken), loggedMessage);
		}

		/// <summary>
		/// Получить список объектов асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TResult">Список объектов</typeparam>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список объектов</returns>
		internal static async Task<TResult[]> ToArrayAndLogErrorAsync<TResult, TLoggedClass>(this IQueryable<TResult> query,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken) where TLoggedClass : class {
			
			return await ContextLogErrorAsync(async () => await query.ToArrayAsync(cancellationToken), loggedMessage);
		}

		/// <summary>
		/// Получить список объектов асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TResult">Список объектов</typeparam>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список объектов</returns>
		internal static async Task<TResult[]> ToArrayAndLogErrorAsync<TResult, TLoggedClass>(this IQueryable<TResult> query,
			CancellationToken cancellationToken) where TLoggedClass : class {
			
			ILogger<TLoggedClass> logger				= MKKContext.LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage	=
				new LoggedMessage<TLoggedClass>(logger, "Ошибка выгрузки в массив из БД./*Тип объекта {TResult}*/", typeof(TResult));

			return await ContextLogErrorAsync(async () => await query.ToArrayAsync(cancellationToken), loggedMessage);
		}

		/// <summary>
		/// Получить объект асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <typeparam name="TSource">Источник</typeparam>
		/// <typeparam name="TKey">Класс ключ</typeparam>
		/// <typeparam name="TElement">Класс элемента</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="keySelector">Функция выбора ключа</param>
		/// <param name="elementSelector">Функция выбора значения словаря</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список объектов</returns>
		internal static async Task<Dictionary<TKey, TElement>> ToDictionaryAndLogErrorAsync<TSource, TLoggedClass, TKey, TElement>
		(this IQueryable<TSource> query, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken) where TLoggedClass : class {
			
			return await ContextLogErrorAsync(async () => await query.
				ToDictionaryAsync(keySelector, elementSelector, cancellationToken), loggedMessage);
		}

		/// <summary>
		/// Получить объект асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TResult">Список объектов</typeparam>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список объектов</returns>
		internal static async Task<TResult> FirstOrDefaultAndLogErrorAsync<TResult, TLoggedClass>(this IQueryable<TResult> query,
			LoggedMessage<TLoggedClass> loggedMessage, CancellationToken cancellationToken) where TLoggedClass : class {
			
			return await ContextLogErrorAsync(async () => await query.FirstOrDefaultAsync(cancellationToken), loggedMessage);
		}

		/// <summary>
		/// Получить объект асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TResult">Список объектов</typeparam>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список объектов</returns>
		internal static async Task<TResult> FirstOrDefaultAndLogErrorAsync<TResult, TLoggedClass>(this IQueryable<TResult> query,
			CancellationToken cancellationToken) where TLoggedClass : class {
			ILogger<TLoggedClass> logger				= MKKContext.LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage	=
				new LoggedMessage<TLoggedClass>(logger, "Ошибка поиска объекта в БД./*Тип объекта {TResult}*/", typeof(TResult));
			
			return await ContextLogErrorAsync(async () => await query.FirstOrDefaultAsync(cancellationToken), loggedMessage);
		}

		/// <summary>
		/// Получить объект асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TResult">Список объектов</typeparam>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Объект</returns>
		internal static async Task<TResult> SingleOrDefaultAndErrorAsync<TResult, TLoggedClass>(
			this IQueryable<TResult> query, LoggedMessage<TLoggedClass> loggedMessage,
			CancellationToken cancellationToken) where TLoggedClass : class {
			
			return await ContextLogErrorAsync(async () => await query.SingleOrDefaultAsync(cancellationToken), loggedMessage);
		}

		/// <summary>
		/// Получить объект асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TResult">Список объектов</typeparam>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="query">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Объект</returns>
		internal static async Task<TResult> SingleOrDefaultAndErrorAsync<TResult, TLoggedClass>(this IQueryable<TResult> query,
			CancellationToken cancellationToken) where TLoggedClass : class {
			ILogger<TLoggedClass> logger = MKKContext.LoggerFactory.CreateLogger<TLoggedClass>();
			LoggedMessage<TLoggedClass> loggedMessage =
				new LoggedMessage<TLoggedClass>(logger, "Ошибка поиска объекта в БД./* Объект {TResult}*/", typeof(TResult));

			return await SingleOrDefaultAndErrorAsync(query, loggedMessage, cancellationToken);
		}

		/// <summary>
		/// Выполнить метод асинхронно, при возникновении исключения, сохранить ошибку.
		/// и параметры, вызвавшие исключение.
		/// </summary>
		/// <typeparam name="TResult">Результат выполнения делегата функции</typeparam>
		/// <typeparam name="TLoggedClass">Логируемый класс</typeparam>
		/// <param name="funcAsync">Асинхронный делегат</param>
		/// <param name="loggedMessage">Описание места и состоянии объекта вызвавшего исключение</param>
		/// <returns>Результат выполнения делегата</returns>
		internal static async Task<TResult> ContextLogErrorAsync<TResult, TLoggedClass>(Func<Task<TResult>> funcAsync,
			LoggedMessage<TLoggedClass> loggedMessage) where TLoggedClass : class {
			try {
				return await funcAsync();
			}
			catch (Exception exception) {
				loggedMessage.LogMessage("Ошибка работы с БД. Исключение: {ContextException}", exception);
				throw;
			}
		}
	}
}
