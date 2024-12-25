import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

// HTTPS sertifikaları için temel klasör yolunu belirle
const baseFolder = env.APPDATA && env.APPDATA !== ''
    ? path.join(env.APPDATA, 'ASP.NET', 'https')
    : path.join(env.HOME || '', '.aspnet', 'https');

// Sertifika adı ve yolları
const certificateName = 'educationmanagementsystem.client';
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// Sertifika klasörü yoksa oluştur
if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

// Sertifikalar yoksa oluştur
if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    const result = child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit' });

    if (result.status !== 0) {
        throw new Error('HTTPS sertifikası oluşturulamadı. Lütfen `dotnet dev-certs https` komutunu manuel olarak çalıştırın.');
    }
}

// Backend hedef URL'si
const target = env.ASPNETCORE_HTTPS_PORT
    ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
    : env.ASPNETCORE_URLS
        ? env.ASPNETCORE_URLS.split(';')[0]
        : 'https://localhost:7214';

// Vite yapılandırması
export default defineConfig({
    plugins: [react()], // React için Vite plugin'i
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url)), // @ ile src klasörüne kısayol
        },
    },
    server: {
        proxy: {
            '/api': {
                target, // Backend API için proxy
                secure: false, // Yerel geliştirme için SSL doğrulamasını devre dışı bırak
                changeOrigin: true, // CORS sorunlarını çözmek için
            },
        },
        port: 51691, // Frontend için port
        https: {
            key: fs.readFileSync(keyFilePath, 'utf8'), // Sertifika anahtarı
            cert: fs.readFileSync(certFilePath, 'utf8'), // Sertifika dosyası
        },
    },
});
