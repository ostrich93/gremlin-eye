﻿namespace gremlin_eye.Server.Enums
{
    public enum ListSorting
    {
        UserOrder,
        GameTitle,
        Popularity,
        ReleaseDate,
        Trending,
        UserRating,
        AvgRating,
        TimePlayed
    }

    public static class ListSortingExtensions
    {
        public static string ToStringValue(this ListSorting listSorting)
        {
            return listSorting.ToString().ToLower();
        }
    }
}
