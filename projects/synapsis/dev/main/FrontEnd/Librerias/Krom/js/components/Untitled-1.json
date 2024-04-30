SI(ROOM("A22.EXISTEIDENTIFICADORGENERAL","PD",RANGO(S55.CVE_IDENTIFICADOR,1,N))="OK",S18.CA_COMPLEMENTO_1*CFDTA,0) + 
SI(Y(ROOM("A22.EXISTEIDENTIFICADORGENERAL","AI",RANGO(S55.CVE_IDENTIFICADOR,1,N))="OK",ROOM("A22.EXISTEIDENTIFICADORGENERAL","5",RANGO(S55.CA_COMPLEMENTO_2,1,N))="OK", S1.CA_TIPO_OPERACION=1), 
    SI( CFDTA>=RED(S1.CA_VALOR_ADUANA*S6.CA_TASA*0.92/1000),CFDTA) ,
    SI(CFDTA>=RED(S1.CA_VALOR_ADUANA*S6.CA_TASA/1000),CFDTA,RED(S1.CA_VALOR_ADUANA*S6.CA_TASA/1000)))
    

SI(S55.CA_CONCEPTO="PRV",SI(S55.CA_IMPORTE=PRV,"OK","BAD"),
                         SI(S55.CA_CONCEPTO="IVA/PRV",SI(S55.CA_IMPORTE=RED(IVA*PRV/100),"OK",
                                                                                     "BAD"),
                                                      SI(S55.CA_CONCEPTO="DTA",SI(CA_IMPORTE=ROOM("A22.CALCULODTAIMPORTE",
                                                                                                  RANGO(S55.CA_CVE_IDENTIFICADOR,1,N),
                                                                                                  RANGO(S55.CA_CVE_IDENTIFICADOR,1,N),
                                                                                                  RANGO(S55.CA_COMPLEMENTO_1,1,N),
                                                                                                  RANGO(S55.CA_COMPLEMENTO_2,1,N),
                                                                                                  S1.CA_TIPO_OPERACION,
                                                                                                  S1.CA_VALOR_ADUANA,
                                                                                                  S6.CA_TASA),"OK",
                                                                                                              "BAD"),
                                                                                 SI(ESBLANCO(S55.CA_IMPORTE),"BAD","OK"))))











SI(ESBLANCO(S44.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA),"BAD","OK")


SI(ESBLANCO(S16.CA_GUIA_MANIFIESTO_BL),SI(S1.CA_TIPO_OPERACION=1,SI(EN(S1.CA_MEDIO_TRANSPORTE,1,4),"BAD",
                                                                                                   "OK"),
                                                                 SI(EN(S1.CA_MEDIO_TRANSPORTE,2,3,6),"BAD",
                                                                                                      "OK")))

SI(S55.CA_CONCEPTO="PRV",SI(S55.CA_IMPORTE=PRV,"OK","BAD"),SI(S55.CA_CONCEPTO="IVA/PRV",SI(S55.CA_IMPORTE=RED(TASAIVA*PRV/100),"OK","BAD"),SI(S55.CA_CONCEPTO="DTA",SI(S55.CA_IMPORTE=ROOM("A22.CALCULODTAIMPORTE",RANGO(S55.CA_CVE_IDENTIFICADOR,1,N), RANGO(S55.CA_COMPLEMENTO_1,1,N), RANGO(S55.CA_COMPLEMENTO_2,1,N), S1.CA_TIPO_OPERACION,S1.CA_VALOR_ADUANA,S6.CA_TASA),"OK","BAD"), SI(S55.CA_IMPORTE=SUMAR(RANGO(RED(S29.CA_IMPORTE_PARTIDA*S29.CA_TASA_PARTIDA/100),1,N)),"OK","BAD"))))



SI(S55.CA_CONCEPTO="PRV",SI(S55.CA_IMPORTE=PRV,"OK","BAD"),
                         SI(S55.CA_CONCEPTO="IVA/PRV",SI(S55.CA_IMPORTE=RED(TASAIVA*PRV/100),"OK",
                                                                                             "BAD"),
                         SI(S55.CA_CONCEPTO="DTA",SI(S55.CA_IMPORTE=ROOM("A22.CALCULODTAIMPORTE",RANGO(S55.CA_CVE_IDENTIFICADOR,1,N), RANGO(S55.CA_COMPLEMENTO_1,1,N), RANGO(S55.CA_COMPLEMENTO_2,1,N), S1.CA_TIPO_OPERACION,S1.CA_VALOR_ADUANA,S6.CA_TASA),"OK","BAD"), SI(S55.CA_IMPORTE=SUMAR(RANGO(RED(S29.CA_IMPORTE_PARTIDA*S29.CA_TASA_PARTIDA/100),1,N)),"OK","BAD"))))


{([S55.CA_IMPORTE.0]) = (ROOM("A22.CALCULODTAIMPORTE", [S55.CA_CVE_IDENTIFICADOR.0], [S55.CA_COMPLEMENTO_1.0], [S55.CA_COMPLEMENTO_2.0], [S1.CA_TIPO_OPERACION.0], [S1.CA_VALOR_ADUANA.0], [S6.CA_TASA.0]))}


SI(O(S1.CA_TIPO_OPERACION=2,S1.CA_CVE_PEDIMENTO="CT"),CFDTAE,SI(ROOM("A22.EXISTEIDENTIFICADORGENERAL","PD",RANGO(S55.CA_CVE_IDENTIFICADOR,1,N))="OK",S55.CA_COMPLEMENTO_1*CFDTA,0) +SI(Y(ROOM("A22.EXISTEIDENTIFICADORGENERAL","AI",RANGO(S55.CA_CVE_IDENTIFICADOR,1,N))="OK",ROOM("A22.EXISTEIDENTIFICADORGENERAL","5",RANGO(S55.CA_COMPLEMENTO_2,1,N))="OK", S1.CA_TIPO_OPERACION=1), SI( CFDTA>=RED(S1.CA_VALOR_ADUANA*S6.CA_TASA*0.92/1000),CFDTA,RED(S1.CA_VALOR_ADUANA*S6.CA_TASA*0.92/1000)) ,SI(CFDTA>=RED(S1.CA_VALOR_ADUANA*S6.CA_TASA/1000),CFDTA,RED(S1.CA_VALOR_ADUANA*S6.CA_TASA/1000))))


SI(O(S1.CA_TIPO_OPERACION=2 , S1.CA_CVE_PEDIMENTO="CT") , CFDTAE , 
                                                          SI(ROOM("A22.EXISTEIDENTIFICADORGENERAL" , "PD" , RANGO(S55.CA_CVE_IDENTIFICADOR , 1 , N))="OK" , S55.CA_COMPLEMENTO_1 * CFDTA , 
                                                                                                                                                                                      0) + 
                                                          SI(Y(ROOM("A22.EXISTEIDENTIFICADORGENERAL" , "AI" , RANGO(S55.CA_CVE_IDENTIFICADOR , 1 , N))="OK" , ROOM("A22.EXISTEIDENTIFICADORGENERAL" , "5" , RANGO(S55.CA_COMPLEMENTO_2 , 1 , N))="OK" , S1.CA_TIPO_OPERACION=1) , SI( CFDTA > =RED(S1.CA_VALOR_ADUANA * S6.CA_TASA * 0.92 / 1000) , CFDTA , RED(S1.CA_VALOR_ADUANA * S6.CA_TASA * 0.92 / 1000)) , SI(CFDTA > =RED(S1.CA_VALOR_ADUANA * S6.CA_TASA / 1000) , CFDTA , RED(S1.CA_VALOR_ADUANA * S6.CA_TASA / 1000))))











