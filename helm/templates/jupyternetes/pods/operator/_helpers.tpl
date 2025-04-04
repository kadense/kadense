{{- define "jupyternetes.pods.operator.fullname" -}}
{{- printf "%s-jpod-op" $.Release.Name }}
{{- end }}

{{- define "jupyternetes.pods.operator.serviceAccountName" -}}
{{- if ne .Values.jupyternetes.pods.operator.serviceAccount.name "" -}}
{{- printf "%s" .Values.jupyternetes.pods.operator.serviceAccount.name }}
{{- else -}}
{{- include "jupyternetes.pods.operator.fullname" . }}
{{- end }}
{{- end }}

{{- define "jupyternetes.pods.operator.image.repository" -}}
{{- if ne .Values.jupyternetes.pods.operator.image.repository "" -}}
{{- printf "%s" .Values.jupyternetes.pods.operator.image.repository }}
{{- else -}}
{{- printf "%s" .Values.image.repository }}
{{- end }}
{{- end }}

{{- define "jupyternetes.pods.operator.image.name" -}}
{{- printf "%s/%s:%s" (include "jupyternetes.pods.operator.image.repository" .) .Values.jupyternetes.pods.operator.image.name .Values.jupyternetes.pods.operator.image.tag }}
{{- end }}