#lab1 <- function(x) {

    custom_gauss_quantile <- function(q) {
        c0 = 2.515517
        c1 = 0.8028538
        c2 = 0.01032
        d1 = 1.432788
        d2 = 0.189269
        d3 = 0.001308
        t = sqrt(log((1 - q) ^ -2))
        t - (c0 + c1 * t + c2 * t ^ 2) / (1 + d1 * t + d2 * t ^ 2 + d3 * t ^ 3)
    }

    custom_student_quantile <- function(gq, v) {
        q1 = (gq ^ 2 + 1) * gq / 4
        q2 = ((5 * gq ^ 2 + 16) * gq ^ 2 + 3) * gq / 96
        q3 = (((3 * gq ^ 2 + 19) * gq ^ 2 + 17) * gq ^ 2 - 15) * gq / 384
        q4 = ((((79 * gq ^ 2 + 776) * gq ^ 2 + 1482) * gq ^ 2 - 1920) * gq ^ 2 - 945) * gq / 92160
        gq + q1 / v + q2 / v ^ 2 + q3 / v ^ 3 + q4 / v ^ 4
    }

    x <- c(528.2, 542.8, 531.2, 597.5, 555.9, 553.2, 539.2, 548.2, 523.5, 561.0,
       546.8, 545.8, 538.4, 541.7, 547.6, 541.9, 551.0, 540.0, 569.8, 529.3,
       524.2, 558.5, 544.0, 545.4, 539.6, 525.5, 592.2, 536.8, 519.9, 505.1,
       536.0, 584.5, 540.3, 544.5, 535.0, 551.3, 558.3, 525.5, 554.7, 542.1
       )

    m <- mean(x)
    s2 <- var(x, y = NULL, na.rm = FALSE)
    s <- sd(x)

    v = length(x) - 1
    a = 0.05
    q = 1 - a

    max_val = max(x)
    min_val = min(x)

    u = abs(max_val - m) / s

    gauss_quantile = custom_gauss_quantile(q)
    student_custom_quantile = custom_student_quantile(gauss_quantile, v)
    students_quintile = qt(0.95, v)

    x_result_1 <- x[which(students_quintile > abs(x - m) / s)]
    x_misses_1 <- x[which(students_quintile <= abs(x - m) / s)]


    x_misses_2 = list()

    for (i in 1:length(x)) {
        elem = as.double(x[1])
        x = x[-1]
        m_without_max = mean(x)
        s_without_max = sd(x)
        k = abs(elem - m_without_max) / s_without_max
        if (k > 2) {
            x_misses_2 = append(x_misses_2, elem)
        }
        x = append(x, elem)
    }


    #result = t.test(x, y = NULL, alternative = c("two.sided"), var.equal = FALSE, conf.level = q, paired = FALSE)

    #miss <- x[which(x < result$conf.int[1] | x > result$conf.int[2])]

    print(sprintf("Mean = %s", m))
    print(sprintf("Variance = %s", s2))
    print(sprintf("Standard deviation = %.3f", s))
    print(sprintf("Confidence level = %.2f", q))
    print(sprintf("Number of degrees of freedom = %d", v))
    print(sprintf("Maximal x = %s", max_val))
    print(sprintf("Minimal x = %s", min_val))
    print(sprintf("Student's quintile = %.3f", students_quintile))
    print(sprintf("Value u = %f", u))
    print(sprintf("Passed values (way 1):"))
    print(x_result_1)
    print(sprintf("Misses (way 1):"))
    print(x_misses_1)
    print(sprintf("Misses (way 2):"))
    print(x_misses_2)

#}
