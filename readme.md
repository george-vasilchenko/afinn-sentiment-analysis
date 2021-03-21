## Sentiment Analysis
Interpretation of results

### Overview
The application is configured to perform two separate analysis iterations using the:
- custom implementation of **AFINN** algorithm
- consumption of **Azure Text Analytics** service

Both iterations examine the same psudonymized dataset

### Results
#### Afinn
Two files were produced during the iteration:
- _afinn_results_raw.csv_ (raw CSV output)
- _afinn_results_table.xlsx_ (table formatted results)

**Value** column represents the output score for a given comment (**Subject** column). The original score value is within -5 to 5 range, the final value in the result files is normaized (range -1 to 1).
**Subject** column contains the original comment.

##### Azure Text Analytics
Two files were produced during the iteration:
- _azure_results_raw.csv_ (raw CSV output)
- _azure_results_table.xlsx_ (table formatted results)

**Average** column is an optimistic approximation of the results obtained from the api response. Originally the output values are in the 0 to 1 range, however, the values in this column are averaged and mapped onto -1 to 1 range using this method:


**Subject** column contains the original comment.