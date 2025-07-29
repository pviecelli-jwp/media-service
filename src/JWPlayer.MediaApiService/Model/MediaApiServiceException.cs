namespace JWPlayer.MediaApiService.Model;
public class MediaApiServiceException(string message, Exception? innerException) : Exception(message, innerException);