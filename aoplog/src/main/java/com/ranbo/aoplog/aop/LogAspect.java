package com.ranbo.aoplog.aop;

import org.aspectj.lang.JoinPoint;
import org.aspectj.lang.ProceedingJoinPoint;
import org.aspectj.lang.annotation.*;
import org.aspectj.lang.reflect.MethodSignature;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;

import java.lang.reflect.Method;

/**
 * 用于观察切面注解执行顺序
 *
 * @Around => @Before => @AfterReturning => @After => @Around
 */
@Aspect
@Component
public class LogAspect {
    private final Logger logger = LoggerFactory.getLogger(LogAspect.class);

    //关联注解
    @Pointcut("@annotation(Log)")
    public void logPointCut() {
    }

    @Before("logPointCut()")
    public void logBeforeTime(JoinPoint joinPoint) throws Throwable {
        logger.info("start before");
        final long start = System.currentTimeMillis();

        MethodSignature signature = (MethodSignature) joinPoint.getSignature();
        Method method = signature.getMethod();

        final long executionTime = System.currentTimeMillis() - start;

        logger.info(joinPoint.getSignature() + " executed before " + executionTime + "ms");
        logger.info("end before");
    }

    @AfterReturning("logPointCut()")
    public void logAfterReturnTime(JoinPoint joinPoint) throws Throwable {
        logger.info("start afterreturn");
        final long start = System.currentTimeMillis();

        MethodSignature signature = (MethodSignature) joinPoint.getSignature();
        Method method = signature.getMethod();

        final long executionTime = System.currentTimeMillis() - start;

        logger.info(joinPoint.getSignature() + " executed afterReturn " + executionTime + "ms");
        logger.info("end afterreturn");
    }

    @Around("logPointCut()")
    public Object logExecutionTime(ProceedingJoinPoint joinPoint) throws Throwable {
        logger.info("start around");
        final long start = System.currentTimeMillis();
        logger.info("before procced");
        final Object proceed = joinPoint.proceed();
        logger.info("after procced");
        final long executionTime = System.currentTimeMillis() - start;

        logger.info(joinPoint.getSignature() + " executed in " + executionTime + "ms");
        logger.info("proceed: " + proceed.toString());
        logger.info("end around");
        return proceed;
    }

    @After("logPointCut()")
    public void logAfterTime(JoinPoint joinPoint) throws Throwable {
        logger.info("start after");
        final long start = System.currentTimeMillis();

        MethodSignature signature = (MethodSignature) joinPoint.getSignature();
        Method method = signature.getMethod();

        final long executionTime = System.currentTimeMillis() - start;

        logger.info(joinPoint.getSignature() + " executed After " + executionTime + "ms");
        logger.info("end after");
    }
}
