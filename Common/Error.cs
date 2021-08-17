//namespace Common;

///// <summary>
///// Klasse für Fehlerbehandlung per Optionen
///// </summary>
//public class Result<R, E>
//{
//    private R? _result;
//    private E? _error;

//    public Result(R? result, E? error)
//    {
//        _result = result;
//        _error  = error;
//    }

//    public static Result<R, E> Ok(R result)
//    {
//        return new Result<R, E>(result, default);
//    }

//    public bool Ok()
//    {
//        return new Result<R, E>(result, default);
//    }

//    public static Result<R, E> Fail(E error)
//    {
//        return new Result<R, E>(default, error);
//    }

//    /// <summary>
//    /// Hilfsmethode für Exceptionhandling um eine mglw. fehlerhaftes Lambda
//    /// </summary>
//    public static Result<R, E> Try(Func<R> func)
//    {
//        try
//        {
//            return new Result<R, E>(func(), default);
//        }
//        catch(Exception e)
//        {
//            return new Result<R, E>(default, e);
//        }
//    }
//}
