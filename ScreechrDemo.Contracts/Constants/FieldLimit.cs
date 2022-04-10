using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreechrDemo.Contracts.Constants
{
    public class FieldLimit
    {
        public const int DEFAULT_PAGE_SIZE = 50;
        public const int DEFAULT_PAGE_NUM = 1;
        public const int MAXIMUM_PAGE_SIZE = 500;

        public const string SortAsc = "asc";
        public const string SortDsc = "dsc";

        public const int MAX_CONTENT_SIZE = 1024;

        public const int MAX_USER_NAME_LENGTH = 80;

        public const int MAX_FIRST_NAME_LENGTH = 100;
        public const int MAX_LAST_NAME_LENGTH = 100;

        public const int EXACT_TOKEN_LENGTH = 32;
        public const int SECRET_TOKEN_MIN_LENGTH = 8;

        public const string ImageUrlRegexPattern = @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$";
    }
}
