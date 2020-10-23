using System;
using System.Threading.Tasks;

namespace NBCH_LIB {
    public static class Extensions {
        /// <summary>
        /// Дождаться окончания выполнения задачи, в случае возникновения исключения, достать его
        /// и пробросить на клиента.
        /// </summary>
        /// <param name="task">Ожидаемая задача</param>
        public static void WaitAndThrowException(this Task task) {
            try {
                task.Wait();
            }
            catch (AggregateException exception) {
                if (exception.InnerException != default)
                    throw exception.InnerException;
                throw;
            }
        }
        
        /// <summary>
        /// Дождаться окончания выполнения задачи и вернуть результат. В случае возникновения исключения,
        /// достать его и пробросить клиенту.
        /// </summary>
        /// <param name="task">Ожидаемая задача</param>
        /// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
        /// <returns>Результат работы задачи</returns>
        public static TResult ResultAndThrowException<TResult>(this Task<TResult> task) {
            try {
                return task.Result;
            }
            catch (AggregateException exception) {
                if (exception.InnerException != default)
                    throw exception.InnerException;
                throw;
            }
        }
    }
}