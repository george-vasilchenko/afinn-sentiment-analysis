
## Sentiment Analysis


### Overview
The application is configured to perform two separate analysis iterations using the:
- custom implementation of **AFINN** algorithm
- consumption of **Azure Text Analytics** service

Both iterations examine the same pseudonymised dataset

### Results Afinn
Two files were produced during the iteration:
- _afinn_results_raw.csv_ (raw CSV output)
- _afinn_results_table.xlsx_ (table formatted results)

**Value** column represents the output score for a given comment (**Subject** column). The original score value is within the -5 to 5 range, the final value in the result files is normalized (range -1 to 1).
**Subject** column contains the original comment.

### Results Azure Text Analytics

Two files were produced during the iteration:

- _azure_results_raw.csv_ (raw CSV output)
- _azure_results_table.xlsx_ (table formatted results)

**Average** column is an optimistic approximation of the results obtained from the api response. Originally the output values are in the 0 to 1 range, however, the values in this column are averaged and mapped to -1 to 1 range using this method:<img width="413" alt="Screenshot 2021-03-21 at 15 40 28" src="https://user-images.githubusercontent.com/65495543/111909009-c95e7580-8a5b-11eb-876d-5b6f6f5eb442.png">

Columns **Negative**, **Neutral**, and **Positive** represent the raw api response values respectively.
**Subject** column contains the original comment.

### Combined Results
Two files are present as combined results:
- _combined_results_raw.csv_ (raw CSV output)
- _combined_results_table.xlsx_ (table formatted results)

The content of these files is built based on the outputs from each of the tests (Afinn and Azure Text Analytics) and compiled in a single text file.
