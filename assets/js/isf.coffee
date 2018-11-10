---
---

$ ->
  $('.isf').each ->
    width = 600
    height = 600
    $(@).append "<iframe src="https://isf.video/sketches/#{@id}/embed" width=\"#{width}\ height=\"#{height}\" style="border: 0"></iframe>"