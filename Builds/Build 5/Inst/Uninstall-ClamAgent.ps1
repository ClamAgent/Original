Disable-TransportAgent -Identity "ClamAgent"
Uninstall-TransportAgent -Identity "ClamAgent"
net stop MSExchangeTransport
net start MSExchangeTransport