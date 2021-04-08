using System;
using System.Collections.Generic;

namespace Domain.Commands
{
    public class Response<T>
    {
        public T Entity { get; private set; }
        public bool IsSuccessful { get; private set; }
        public ResponseCode ResponseCode { get; private set; }
        public List<String> Errors { get; private set; }

        private Response(T entity, bool success, ResponseCode responseCode, List<string> errors)
        {
            Entity = entity;
            IsSuccessful = success;
            ResponseCode = responseCode;
            Errors = errors ?? new();
        }

        public static Response<T> Created(T entity)
        {
            return new(entity, true, ResponseCode.CREATED, null);
        }

        public static Response<T> ModelError(T entity, List<string> errors)
        {
            return new(entity, false, ResponseCode.MODEL_ERROR, errors);
        }

        public static Response<T> Ok(T entity)
        {
            return new(entity, true, ResponseCode.OK, null);
        }

        public static Response<T> NotFound(T entity, List<string> errors)
        {
            return new(entity, false, ResponseCode.NOT_FOUND, errors);
        }
    }

    public enum ResponseCode
    {
        OK,
        CREATED,
        MODEL_ERROR,
        NOT_FOUND,
    }
}