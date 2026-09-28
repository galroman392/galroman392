package com.desk;

import android.content.Context;
import android.os.Handler;
import android.os.Looper;
import android.webkit.WebSettings;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.CountDownLatch;
import java.util.concurrent.TimeUnit;

public final class DeskReferrer {
    private static final List<String> logs = Collections.synchronizedList(new ArrayList<String>());
    private static volatile String installReferrer = "";
    private static boolean loaded;

    private DeskReferrer() {
    }

    public static synchronized void load() {
        if (loaded)
            return;
        System.loadLibrary("desk");
        loaded = true;
    }

    public static void addLog(String message) {
        if (message != null)
            logs.add(message);
    }

    public static String drainLogs() {
        synchronized (logs) {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < logs.size(); i++) {
                if (i > 0)
                    builder.append('\n');
                builder.append(logs.get(i));
            }
            logs.clear();
            return builder.toString();
        }
    }

    public static native String pull(
            String token,
            String host,
            String lesson,
            String project,
            String player,
            String bell,
            String packageName,
            String device,
            String version,
            String os,
            String model,
            String gpu,
            String vendor);

    public static void setReferrer(String value) {
        installReferrer = value == null ? "" : value;
    }

    public static String referrer(Context context) {
        String value = installReferrer;
        return value == null ? "" : value;
    }

    public static String agent(final Context context) {
        if (Looper.myLooper() == Looper.getMainLooper())
            return readAgent(context);

        final String[] value = new String[] { "" };
        final CountDownLatch latch = new CountDownLatch(1);
        new Handler(Looper.getMainLooper()).post(new Runnable() {
            @Override
            public void run() {
                try {
                    value[0] = readAgent(context);
                } catch (Throwable ignored) {
                } finally {
                    latch.countDown();
                }
            }
        });
        try {
            latch.await(3, TimeUnit.SECONDS);
        } catch (InterruptedException ignored) {
        }
        return value[0] == null ? "" : value[0];
    }

    private static String readAgent(Context context) {
        try {
            String agent = WebSettings.getDefaultUserAgent(context);
            return agent == null ? "" : agent;
        } catch (Throwable ignored) {
            return "";
        }
    }
}
