library(ggplot2)

lmp <- function(modelobject) {
    if (class(modelobject) != "lm") stop("Not an object of class 'lm' ")
    f <- summary(modelobject)$fstatistic
    p <- pf(f[1], f[2], f[3], lower.tail = F)
    attributes(p) <- NULL
    return(p)
}

x <- c(1, 1.2,1.4,1.6,1.8,2,2.2,2.4,2.6,2.8,3,3.2,3.4,3.6,3.8,4,4.2,4.4,4.6,4.8,5,10,15,20,25,30,35,40,45,50,55,60,65,70,75,80,85,90,95,100,105,110)
y <- c(7.19,7.466,7.912,8.495,8.727,8.842,9.673,9.622,10.09,10.744,10.988,11.028,11.897,12.311,12.737,13.054,13.166,13.956,13.949,14.562,15.312,61.53,70.8,71.05,86.21,86.54,91.95,102.44,109.23,115.56,125.26,126.35,138.1,140.17,149.14,156.95,161.53,166.69,172.72,179.69,184.44,193.05)


formula = y ~ 1 + x
result = lm(formula = formula)
pval = lmp(result)
ss = summary(result)
deg = 1

while (pval > 0.01 || ss$adj.r.squared < 0.99 ) {
    deg = deg + 1
    formula = update(formula,.~poly(x,deg,raw = TRUE))
    result = update(result, formula = formula)
    pval = lmp(result)
    ss = summary(result)
}
deg
ggplot(data = NULL, aes(x, y)) + geom_point() + geom_smooth(method = "lm", formula = formula)
summary(result)