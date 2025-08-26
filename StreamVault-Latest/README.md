# 🚀 StreamVault - Latest Version

## ✨ Nuove Funzionalità Aggiunte

### 💾 **Salvataggio Configurazioni Automatico**
- ✅ Tutte le impostazioni vengono salvate automaticamente
- ✅ Configurazione persistente tra riavvii
- ✅ File salvato in: `%AppData%\StreamVault\config.json`

### 🐛 **Sistema Debug FFmpeg Completo**
- ✅ **Pulsante "Debug FFmpeg"** nella finestra MultiStream
- ✅ **Console di debug** con test in tempo reale
- ✅ **Informazioni FFmpeg**: status, versione, path
- ✅ **Test Commands**: 
  - Test Capture (cattura schermo)
  - Test SRT (streaming)
  - Show Devices (dispositivi disponibili)

### 🖥️ **Interfaccia Migliorata**
- ✅ **Finestra MultiStream ingrandita** (1200x800 pixel)
- ✅ **Ridimensionabile** con maximize/minimize
- ✅ **Layout ottimizzato** per mostrare tutte le informazioni

### 🔧 **Logging Avanzato**
- ✅ **FFmpeg commands completi** nel log
- ✅ **Debug in tempo reale** durante lo streaming
- ✅ **Informazioni dettagliate** su errori e problemi

## 🎯 Come Usare il Debug

1. **Apri l'app**: `StreamVault.exe`
2. **Vai su MultiStream** dalla finestra principale
3. **Clicca "Debug FFmpeg"** per aprire la console
4. **Testa FFmpeg**:
   - `Test Capture` - Verifica la cattura schermo
   - `Test SRT` - Verifica lo streaming SRT
   - `Show Devices` - Mostra dispositivi disponibili

## 📋 Risoluzione Problemi

### FFmpeg non cattura?
- Apri Debug FFmpeg
- Clicca "Test Capture"
- Controlla i messaggi di errore nella console

### SRT non funziona?
- Apri Debug FFmpeg  
- Clicca "Test SRT"
- Verifica l'output per errori di rete

### Monitor non rilevati?
- Apri Debug FFmpeg
- Clicca "Show Devices"
- Controlla se i monitor sono visibili

## 🔧 Requisiti

- Windows 10/11
- FFmpeg installato (l'app può scaricarlo automaticamente)
- .NET 8.0 Runtime (incluso nella build)

## 📁 File Configurazione

Le impostazioni vengono salvate automaticamente in:
```
C:\Users\[TuoNome]\AppData\Roaming\StreamVault\config.json
```

Questo include:
- Host e porta SRT
- URL Chrome
- Impostazioni auto-start
- Monitor virtuali creati

---

**Versione**: Latest (26 agosto 2025)
**Funzionalità**: Multi-monitor streaming, Debug completo, Auto-save
