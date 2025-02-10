# Temporal.AsOf.ShowCase

[OpenAI](https://img.shields.io/badge/OpenAI-4B0082?style=for-the-badge&logo=openai&logoColor=white) ![Perplexity AI](https://img.shields.io/badge/Perplexity%20AI-FF8C00?style=for-the-badge&logo=perplexity&logoColor=white) ![DeepSeek](https://img.shields.io/badge/DeepSeek-1E90FF?style=for-the-badge&logo=deepseek&logoColor=white) 
![Llama AI](https://img.shields.io/badge/Llama%20AI-39FF14?style=for-the-badge&logo=meta&logoColor=black)


- ### Known knowns - Known Unknowns - Unknown Unknowns <br><br> https://github.com/Xll-ai/Xll.OpenAI.DeepSeek/blob/main/AdHoc/KnownKnowns.md <br><br>![OpenAI](https://img.shields.io/badge/OpenAI-4B0082?style=for-the-badge&logo=openai&logoColor=white) ![DeepSeek](https://img.shields.io/badge/DeepSeek-1E90FF?style=for-the-badge&logo=deepseek&logoColor=white)

- ### Trump / Gaza <br><br> https://github.com/Xll-ai/Xll.OpenAI.DeepSeek/blob/main/AdHoc/Trump.Gaza.md <br><br> ![OpenAI](https://img.shields.io/badge/OpenAI-4B0082?style=for-the-badge&logo=openai&logoColor=white)   ![DeepSeek](https://img.shields.io/badge/DeepSeek-1E90FF?style=for-the-badge&logo=deepseek&logoColor=white)


- ### Dallas - JR Ewing / Trump <br><br> (https://github.com/Xll-ai/Xll.OpenAI.DeepSeek/blob/main/AdHoc/Trump.JR.Dallas.md)<br><br>![OpenAI](https://img.shields.io/badge/OpenAI-4B0082?style=for-the-badge&logo=openai&logoColor=white) ![DeepSeek](https://img.shields.io/badge/DeepSeek-1E90FF?style=for-the-badge&logo=deepseek&logoColor=white)



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



### Question 1

>Concept of AsOf
>
>For use
>
>Winding back history
>Financial Information / prices that change over time
>Animation (images valid from milliseconds)
>Repeating cycle of images (animated gif)
>
>We have an API
>
>That does
>
>GetData(DateTime asOf)
>
>and
>
>GetDataDelta(DateTime asOfStart, DateTime asOfEnd)
>
>(change of image, price, data etc)
>
>Internally this may well map to 
>
>Integer (index to cache or database)
>
>Or to a math formula
>
>or to a byte[] or image[]
>
>or to some text

### Question 2 - Follow up



>can you write an implementation in C#
>
>
>ok we need to randomly add data
>
>Start a clock at say midnight
>We simulate 5 Minutes
>
>every 5-10 seconds
>
>Either a 
>(string ID-Image, byte[] Image)
> (string ID-Prices, Dictionary<string, double?>) of prices
>or
>a a (string ID-JSON, string JSON)
>arrives
>
>1/3 probability of each type of data
>
>50/50 probability that new data item has same as ID as we have already stored
>ID's a
>
>We cache this data in Memory
>
>Then we randomly access
>
>using
>
>GetData(DateTime asOf)
>
>and
>
>GetDataDelta(DateTime asOfStart, DateTime asOfEnd)

>This has to be threadsafe

>Can we do actual Deltas

>Number of changes is good

>I guess we can just pass back the start value and end value and let use do his own delta calculation
