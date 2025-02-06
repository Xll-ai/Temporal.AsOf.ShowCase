# Temporal.AsOf.ShowCase

A demo project illustrating “as of” (time-based) retrieval for **images/video**, **financial price data**, and **JSON/text**.

## What Is This?

This project simulates incoming data records of three types:

1. **Images** (represented as `byte[]`, as if each is a frame)
2. **Financial Prices** (`Dictionary<string, double?>`)
3. **JSON/Text** (simple JSON strings)

You can retrieve records "as of" any particular time or get all records in a time range (a "delta"). The **TimeBasedDataStore** class handles storage and querying of these versioned data records.

## How to Run

1. Open `TemporalAsOfShowcase.sln` in Visual Studio (2022 or later).
2. Press **F5** or **Ctrl+F5** to run the console app.
3. Observe the output in the console window, which shows data arriving in random intervals and occasional "as of" queries.

