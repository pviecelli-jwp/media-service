namespace JWPlayer.MediaApi.Model;
public class MediaApiException(string message, Exception? innerException) : Exception(message, innerException);