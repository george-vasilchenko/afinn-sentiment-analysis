
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

**Average** column is an optimistic approximation of the results obtained from the api response. Originally the output values are in the 0 to 1 range, however, the values in this column are averaged and mapped to -1 to 1 range using this method:
$out_{avg}$ - averaged result value, $-1 \le out_{avg} \le 1$;
$x_{neg}$ - negative part, $0 \le x \le 1$;
$x_{neu}$ - neutral part, $0 \le x \le 1$;
$x_{pos}$ - positive part, $0 \le x \le 1$;
$out_{avg} = (x_{pos}+\frac{1}{2}x_{neu}) -(x_{neg}+\frac{1}{2}x_{neu})$;
Columns **Negative**, **Neutral**, and **Positive** represent the raw api response values respectively.
**Subject** column contains the original comment.

### Combined Results
Two files are present as combined results:
- _combined_results_raw.csv_ (raw CSV output)
- _combined_results_table.xlsx_ (table formatted results)

The content of these files is built based on the outputs from each of the tests (Afinn and Azure Text Analytics) and compiled in a single text file.