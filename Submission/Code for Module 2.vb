Sub stocks():

    Dim i As Long ' row number
    Dim cell_vol As LongLong ' contents of column G (cell volume)
    Dim vol_total As LongLong ' what is going to go in column L
    Dim ticker As String ' what is going to go in column I

    Dim k As Long ' leaderboard row
    
    Dim ticker_close As Double
    Dim ticker_open As Double
    Dim price_change As Double
    Dim percent_change As Double
    Dim lastRow As Double 'only declare once
    
    Dim ws As Worksheet
    
    Dim greatest_volume As LongLong
    Dim greatest_percent As Double
    Dim lowest_percent As Double
    
    Dim greatest_volume_ticker As String
    Dim greatest_percent_ticker As String
    Dim lowest_percent_ticker As String
    
    
    For Each ws In ThisWorkbook.Worksheets
    
        ' xpert
        lastRow = ws.Cells(ws.Rows.Count, 1).End(xlUp).Row
    
        vol_total = 0
        k = 2
        
        ' Write Leaderboard Columns
        ws.Range("I1").Value = "Ticker"
        ws.Range("J1").Value = "Quarterly Change"
        ws.Range("K1").Value = "Percent Change"
        ws.Range("L1").Value = "Volume"
        
        
        ' assign open for first ticker
        ticker_open = ws.Cells(2, 3).Value
    
        For i = 2 To lastRow:
            cell_vol = ws.Cells(i, 7).Value
            ticker = ws.Cells(i, 1).Value
    
            ' LOOP rows 2 to 254
            ' check if next row ticker is DIFFERENT
            ' if the same, then we only need to add to the vol_total
            ' if DIFFERENT, then we need add last row, write out to the leaderboard
            ' reset the vol_total to 0
    
            If (ws.Cells(i + 1, 1).Value <> ticker) Then
                ' OMG we have a different ticker, panic!!
                vol_total = vol_total + cell_vol
                
                ' get the closing price of the ticker
                ticker_close = ws.Cells(i, 6).Value
                price_change = ticker_close - ticker_open
                
                ' Check if open price is 0 first - to protect from Kevin the Hacker
                If (ticker_open > 0) Then
                    percent_change = price_change / ticker_open
                Else
                    percent_change = 0
                End If
    
                ws.Cells(k, 9).Value = ticker
                ws.Cells(k, 10).Value = price_change
                ws.Cells(k, 11).Value = percent_change
                ws.Cells(k, 12).Value = vol_total
                
                ' formatting
                If (price_change > 0) Then
                    ws.Cells(k, 10).Interior.ColorIndex = 4 ' Green
                ElseIf (price_change < 0) Then
                    ws.Cells(k, 10).Interior.ColorIndex = 3 ' Red
                Else
                    ws.Cells(k, 10).Interior.ColorIndex = 2 ' White
                End If
                
                ' formatting
                If (percent_change > 0) Then
                    ws.Cells(k, 11).Interior.ColorIndex = 4 ' Green
                ElseIf (percent_change < 0) Then
                    ws.Cells(k, 11).Interior.ColorIndex = 3 ' Red
                Else
                    ws.Cells(k, 11).Interior.ColorIndex = 2 ' White
                End If
                
                ' second leaderboard
                If ticker = ws.Cells(2, 1).Value Then
                    ' init the variables
                    greatest_volume = vol_total
                    greatest_percent = percent_change
                    lowest_percent = percent_change
                    greatest_volume_ticker = ticker
                    greatest_percent_ticker = ticker
                    lowest_percent_ticker = ticker
                Else
                    ' compare the variables
                    If vol_total > greatest_volume Then
                        greatest_volume = vol_total
                        greatest_volume_ticker = ticker
                    End If
                    
                    If percent_change > greatest_percent Then
                        greatest_percent = percent_change
                        greatest_percent_ticker = ticker
                    End If
                    
                    If percent_change < lowest_percent Then
                        lowest_percent = percent_change
                        lowest_percent_ticker = ticker
                    End If
                End If
                
                ' reset
                vol_total = 0
                k = k + 1
                ticker_open = ws.Cells(i + 1, 3).Value ' look ahead to get next ticker open
                ' we just add to the total
            Else
                vol_total = vol_total + cell_vol
            End If
        Next i
        
        ' Style my leaderboard
        ws.Columns("K:K").NumberFormat = "0.00%"
        ws.Columns("I:L").AutoFit
        
        ' write second leaderboard
        ws.Range("Q2").Value = greatest_percent
        ws.Range("Q3").Value = lowest_percent
        ws.Range("Q4").Value = greatest_volume
        
        ws.Range("P2").Value = greatest_percent_ticker
        ws.Range("P3").Value = lowest_percent_ticker
        ws.Range("P4").Value = greatest_volume_ticker
        
        'Style second leaderboard with help from xpert
        ws.Range("Q2").NumberFormat = "0.00%"
        ws.Range("Q3").NumberFormat = "0.00%"
        ws.Range("Q4").NumberFormat = "General"
        ws.Range("O2").Value = "Greatest % Increase"
        ws.Range("O3").Value = "Greatest % Decrease"
        ws.Range("O4").Value = "Greatest Total Volume"
        ws.Range("P1").Value = "Ticker"
        ws.Range("Q1").Value = "Value"
        ws.Columns("O:Q").AutoFit
    
    Next ws
End Sub

Sub reset()
    Dim ws As Worksheet
    
    For Each ws In ThisWorkbook.Worksheets
        ws.Range("I:Q").Value = ""
        ws.Range("I:L").Interior.ColorIndex = 2
    Next ws
End Sub
